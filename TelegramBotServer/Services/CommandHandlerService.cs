using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotServer.Enums;
using TelegramBotServer.Model;
using TelegramBotServer.Repository;
using InlineKeyboardLine = System.Collections.Generic.List<Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton>;


namespace TelegramBotServer.Services
{
    public class CommandHandlerService
    {
        private readonly ITelegramBotClient _bot;
        private readonly ISubscriberRepository _subscriberRepository;
        private readonly IEventRepository _eventRepository;
        private readonly SubscriberSessions _sessions;
        private readonly ILogger<CommandHandlerService> _logger;
        private const int CountVisibleButtons = 4;

        public CommandHandlerService(ITelegramBotClient bot,
            ILogger<CommandHandlerService> logger,
            ISubscriberRepository subscriberRepository,
            IEventRepository eventRepository,
            SubscriberSessions sessions
            )
        {
            _bot = bot;
            _logger = logger;
            _subscriberRepository = subscriberRepository;
            _eventRepository = eventRepository;
            _sessions = sessions;
        }

        public async Task ProcessUpdate(Update update)
        {
            var handler = update.Type switch
            {
                #pragma warning disable CS8602 
                UpdateType.Message => update.Message.Text is not null && update.Message.Text.StartsWith("/") ?
                    ProcessCommand(update.Message ): 
                    ProcessText(update.Message, _sessions),
                #pragma warning restore CS8602
                UpdateType.CallbackQuery => BotOnCallbackQueryReceived(update.CallbackQuery!, _sessions),
                _ => UnknownUpdateHandlerAsync(update)
            };

            try
            {
                await handler;
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(exception);
            }
        }

        private async Task ProcessCommand(Message message)
        {
            _logger.LogInformation("Receive message type: {messageType}", message.Type);
            if (message.Type != MessageType.Text)
                return;

            var action = message.Text!.Split(' ')[0] switch
            {
                "/help" => SendHelpInformation(_bot, message),
                "/subscribe" => Subscribe(_bot, message, _subscriberRepository),
                "/unsubscribe" => Unsubscribe(_bot, message, _subscriberRepository),
                "/plan" => Plan(_bot, message, _sessions, _subscriberRepository),

                _ => SendHelpInformation(_bot, message)
            };

            Message sentMessage = await action;
            _logger.LogInformation("The message was sent with id: {sentMessageId}", sentMessage.MessageId);

            static async Task<Message> SendHelpInformation(ITelegramBotClient bot, Message message)
            {
                return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                      text: "Help information:\n/subscribe - For subscribe on notification\n" +
                                                      "/unsubscribe - For unsubscribe on notification\n/plan - For plan new event");
            }

            static async Task<Message> Subscribe(ITelegramBotClient bot, Message message, ISubscriberRepository repository)
            {
                var sub = repository.GetSubscribers()?.FirstOrDefault(s => s.UserId == message.From?.Id);
                if (sub is not null)
                    return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                      text: "You are already subscribed to notifications!");
                repository.AddSubscriber(new Model.Subscriber
                {
                    UserId = message.From?.Id ?? message.Chat.Id,
                    ChatId = message.Chat.Id,
                });
                return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                      text: "You successfully subscribed on notifications");
            }

            static async Task<Message> Unsubscribe(ITelegramBotClient bot, Message message, ISubscriberRepository repository)
            {
                var senderId = message.From?.Id ?? message.Chat.Id;
                var sub = repository.GetSubscribers()?.FirstOrDefault(s => s.UserId == senderId);
                if (sub is null)
                    return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                      text: "You have not been subscribed to notifications");
                repository.RemoveSubscriber(sub.Id);
                return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                      text: "You successfully unsubscribed on notifications");
            }

            static async Task<Message> Plan(
                ITelegramBotClient bot,
                Message message,
                SubscriberSessions sessions,
                ISubscriberRepository repository)
            {
                var sub = repository.GetSubscribers()?.FirstOrDefault(s => s.UserId == message.Chat.Id);
                if (sub is null)
                {
                    return await bot.SendTextMessageAsync(message.Chat.Id, "Before plan any event you must be subscribed!");
                }

                var sendKeyboard = SendDayInlineKeyboard(bot, message, DateTime.Now);

                if (!sessions.Sessions.ContainsKey(message.Chat.Id))
                {
                    sessions[message.Chat.Id] = new SubscriberSession
                    {
                        CreationTime = DateTime.Now,
                        CurrentChoice = SubscriberSession.ChoiceType.Day,
                        ViewedTime = DateTime.Now
                    };
                }
                else
                {
                    sessions[message.Chat.Id].CreationTime = DateTime.Now;
                    sessions[message.Chat.Id].ViewedTime = DateTime.Now;
                    sessions[message.Chat.Id].CurrentChoice = SubscriberSession.ChoiceType.Day;
                }
                return await sendKeyboard;
            }
        }

        private async Task<Message> ProcessText(Message message, SubscriberSessions sessions)
        {
            var session = sessions.Sessions.ContainsKey(message.Chat.Id) ? sessions[message.Chat.Id] : null ;
            if (session is null || session.ViewedTime is null || session.CurrentChoice != SubscriberSession.ChoiceType.Message )
                return await _bot.SendTextMessageAsync(message.Chat.Id, "Use command from /help");

            var sub = _subscriberRepository.GetSubscribers()
                ?.FirstOrDefault(s => s.UserId == message.Chat.Id);
            if (sub is null)
                return await _bot.SendTextMessageAsync(message.Chat.Id, "You must be subscribed"); ;

            var eventId = _eventRepository.AddEvent(new Event
            {
                Deadline = new DateTime(
                    session.ViewedTime.Value.Year,
                    session.ViewedTime.Value.Month,
                    session.ViewedTime.Value.Day,
                    session.ViewedTime.Value.Hour,
                    session.ViewedTime.Value.Minute,
                    session.ViewedTime.Value.Second
                ),
                Notified = false,
                Reminder = 60,
                SubscriberId = sub.Id,
                Message = message.Text
            });
            sub.EventsId?.Add(eventId);
            _subscriberRepository.ChangeSubscriber(sub.Id, sub);
            session.CurrentChoice = null;
            return await _bot.SendTextMessageAsync(message.Chat.Id, $"You have successfully scheduled a new event.\n" +
                $"The reminder will come in {60} minutes");
        }

        private async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery, SubscriberSessions sessions)
        {
            if (callbackQuery.Data is not null)
            {
                var callbackData = JsonSerializer.Deserialize<CallbackData>(callbackQuery.Data);
                var session = sessions[callbackQuery.From.Id];
                if (session is null || callbackQuery.Message is null || callbackData?.Data is null)
                    return;
                if (callbackData?.Type == CallbackDataType.Plan)
                {
                    if (session.ViewedTime is null)
                        session.ViewedTime = DateTime.Now;
                    switch (session.CurrentChoice)
                    {
                        case SubscriberSession.ChoiceType.Day:
                            if (callbackData.Data == ">>")
                            {
                                await SendDayInlineKeyboard(_bot, callbackQuery.Message,
                                    session.ViewedTime.Value.AddDays(CountVisibleButtons), true);
                                session.ViewedTime = session.ViewedTime.Value.AddDays(CountVisibleButtons);
                            }
                            else if (callbackData.Data == "<<")
                            {
                                await SendDayInlineKeyboard(_bot, callbackQuery.Message,
                                    session.ViewedTime.Value.Subtract(TimeSpan.FromDays(CountVisibleButtons)), true);
                                session.ViewedTime = session.ViewedTime.Value.Subtract(TimeSpan.FromDays(CountVisibleButtons));
                            }
                            else
                            {
                                session.CurrentChoice = SubscriberSession.ChoiceType.Hour;
                                session.ViewedTime = DateTime.Parse(callbackData.Data);
                                var unRoundedTime = session.ViewedTime.Value;
                                var roundedTime = unRoundedTime.AddMinutes(unRoundedTime.Minute > 30 ?
                                    -(unRoundedTime.Minute - 30) : -unRoundedTime.Minute);
                                session.ViewedTime = roundedTime;
                                await SendHourInlineKeyboard(_bot, callbackQuery.Message, roundedTime, true);
                            }

                            break;
                        case SubscriberSession.ChoiceType.Hour:
                            if (callbackData.Data == ">>")
                            {
                                await SendHourInlineKeyboard(_bot, callbackQuery.Message,
                                    session.ViewedTime.Value.AddMinutes(30 * CountVisibleButtons), true);
                                session.ViewedTime = session.ViewedTime.Value.AddMinutes(30 * CountVisibleButtons);
                            }
                            else if (callbackData.Data == "<<")
                            {
                                await SendHourInlineKeyboard(_bot, callbackQuery.Message,
                                    session.ViewedTime.Value.Subtract(TimeSpan.FromMinutes(30 * CountVisibleButtons)), true);
                                session.ViewedTime = session.ViewedTime.Value.Subtract(TimeSpan.FromMinutes(30 * CountVisibleButtons));
                            }
                            else
                            {
                                session.ViewedTime = DateTime.Parse(callbackData.Data);
                                session.CurrentChoice = SubscriberSession.ChoiceType.Message;
                                if (callbackQuery.Message is not null)
                                    await _bot.DeleteMessageAsync(callbackQuery.From.Id, callbackQuery.Message.MessageId);
                                await _bot.SendTextMessageAsync(callbackQuery.From.Id, "Please enter event message");
                            }
                            break;
                    }
                }
                else if (callbackData?.Type == CallbackDataType.Notification)
                {
                    var eventId = sessions[callbackQuery.From.Id].NotificatedEventId[callbackQuery.Message.MessageId];
                    if (int.Parse(callbackData.Data) == 0)
                    {
                        _eventRepository.RemoveEvent(eventId);
                        var sub = _subscriberRepository.GetSubscribers()?.FirstOrDefault(s => s.UserId == callbackQuery.From.Id);
                        if (sub is not null)
                        {
                            sub.EventsId?.Remove(eventId);
                            _subscriberRepository.ChangeSubscriber(sub.Id, sub);
                        }
                        await _bot.AnswerCallbackQueryAsync(callbackQueryId: callbackQuery.Id, text: "Event was taken");
                    }
                    else
                    {
                        var chEvent = _eventRepository.GetEvent(eventId);
                        if (chEvent is not null)
                        {
                            chEvent.Reminder = int.Parse(callbackData.Data);
                            chEvent.Notified = false;
                            _eventRepository.ChangeEvent(eventId, chEvent);
                            await _bot.AnswerCallbackQueryAsync(callbackQueryId: callbackQuery.Id, text: "Event was postponed");
                        }
                    }
                    sessions[callbackQuery.From.Id].NotificatedEventId.Remove(callbackQuery.Message.MessageId);
                    if (callbackQuery.Message is not null)
                        await _bot.DeleteMessageAsync(callbackQuery.From.Id, callbackQuery.Message.MessageId);
                }
            }

        }

        static private async Task<Message> SendDayInlineKeyboard(ITelegramBotClient bot,
            Message message, DateTime currentDay, bool update = false)
        {
            var inlineKeyboardLine = new InlineKeyboardLine();

            inlineKeyboardLine.Add(InlineKeyboardButton.WithCallbackData("<<",
                JsonSerializer.Serialize(new CallbackData { Type = CallbackDataType.Plan, Data = "<<" })));
            for (var i = currentDay; i < currentDay.AddDays(CountVisibleButtons); i = i.AddDays(1))
            {
                inlineKeyboardLine.Add(InlineKeyboardButton.WithCallbackData(i.Day.ToString(),
                    JsonSerializer.Serialize(new CallbackData { Type = CallbackDataType.Plan, Data = $"{i}" })));
            }
            inlineKeyboardLine.Add(InlineKeyboardButton.WithCallbackData(">>",
                JsonSerializer.Serialize(new CallbackData { Type = CallbackDataType.Plan, Data = ">>" })));
            InlineKeyboardMarkup inlineKeyboard = new(new[] { inlineKeyboardLine });

            if (update)
                return await bot.EditMessageReplyMarkupAsync(message.Chat.Id, message.MessageId, inlineKeyboard);
            else
                return await bot.SendTextMessageAsync(message.Chat.Id, "Choose day of event", replyMarkup: inlineKeyboard);
        }

        static private async Task<Message> SendHourInlineKeyboard(ITelegramBotClient bot,
            Message message, DateTime currentMinutes, bool update = false)
        {
            var inlineKeyboardLine = new InlineKeyboardLine();

            inlineKeyboardLine.Add(InlineKeyboardButton.WithCallbackData("<<",
                JsonSerializer.Serialize(new CallbackData { Type = CallbackDataType.Plan, Data = "<<" })));
            for (var i = currentMinutes; i < currentMinutes.AddMinutes(30 * CountVisibleButtons); i = i.AddMinutes(30))
            {
                inlineKeyboardLine.Add(InlineKeyboardButton.WithCallbackData(i.ToString("HH:mm").ToString(),
                    JsonSerializer.Serialize(new CallbackData { Type = CallbackDataType.Plan, Data = $"{i}" })));
            }
            inlineKeyboardLine.Add(InlineKeyboardButton.WithCallbackData(">>",
                JsonSerializer.Serialize(new CallbackData { Type = CallbackDataType.Plan, Data = ">>" })));
            InlineKeyboardMarkup inlineKeyboard = new(new[] { inlineKeyboardLine });

            if (update)
                return await bot.EditMessageReplyMarkupAsync(message.Chat.Id, message.MessageId, inlineKeyboard);
            else
                return await bot.SendTextMessageAsync(message.Chat.Id, "Choose time of event", replyMarkup: inlineKeyboard);
        }

        private Task UnknownUpdateHandlerAsync(Update update)
        {
            _logger.LogInformation("Unknown update type: {updateType}", update.Type);
            return Task.CompletedTask;
        }

        private Task HandleErrorAsync(Exception exception)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogInformation("HandleError: {errorMessage}", errorMessage);
            return Task.CompletedTask;
        }
    }
}
