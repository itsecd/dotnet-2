using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotServer.Repository;

namespace TelegramBotServer.Services
{
    public class CommandHandlerService
    {
        private readonly ITelegramBotClient _bot;
        private readonly ISubscriberRepository _subscriberRepository;
        private readonly IEventRepository _eventRepository;
        private readonly PlanSessions _sessions;
        private readonly ILogger<CommandHandlerService> _logger;

        public CommandHandlerService(ITelegramBotClient bot,
            ILogger<CommandHandlerService> logger,
            ISubscriberRepository subscriberRepository,
            IEventRepository eventRepository,
            PlanSessions sessions
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
                UpdateType.Message => ProcessCommand(update.Message!),
                UpdateType.CallbackQuery => TestCallbackQueryReceived(update.CallbackQuery!, _sessions),
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
                "/test" => SendInlineKeyboard(_bot, message, _sessions),

                _ => SendHelpInformation(_bot, message)
            };

            Message sentMessage = await action;
            _logger.LogInformation("The message was sent with id: {sentMessageId}", sentMessage.MessageId);

            static async Task<Message> SendHelpInformation(ITelegramBotClient bot, Message message)
            {
                return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                      text: "Help information:\n/subscribe - For subscribe on notification\n" +
                                                      "/unsubscribe - For unsubscribe on notification");
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

            static async Task<Message> SendInlineKeyboard(ITelegramBotClient bot, Message message, PlanSessions sessions)
            {
                InlineKeyboardMarkup inlineKeyboard = new(
                new[]
                {
                    // first row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("<<", "<<"),
                        InlineKeyboardButton.WithCallbackData("1.1", "11"),
                        InlineKeyboardButton.WithCallbackData("1.2", "12"),
                        InlineKeyboardButton.WithCallbackData(">>", ">>"),
                    }
                });

                sessions.sessions.Add(message.From.Id, new PlanSession { PageNum = 1 });

                return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                      text: "Choose",
                                                      replyMarkup: inlineKeyboard);
            }

        }


        private async Task TestCallbackQueryReceived(CallbackQuery callbackQuery, PlanSessions sessions)
        {
            if (callbackQuery.Data == ">>" )
            {
                if (sessions.sessions[callbackQuery.From.Id].PageNum == 1)
                {
                    sessions.sessions[callbackQuery.From.Id].PageNum = 2;
                    InlineKeyboardMarkup inlineKeyboardNew = new(
                        new[]
                        {
                            // first row
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("<<", "<<"),
                                InlineKeyboardButton.WithCallbackData("2.1", "21"),
                                InlineKeyboardButton.WithCallbackData("2.2", "22"),
                                InlineKeyboardButton.WithCallbackData(">>", ">>"),
                            }
                        });
                    _bot.EditMessageReplyMarkupAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId, inlineKeyboardNew);
                }
            }
            else if (callbackQuery.Data == "<<")
            {
                if (sessions.sessions[callbackQuery.From.Id].PageNum == 2)
                {
                    sessions.sessions[callbackQuery.From.Id].PageNum = 1;
                    InlineKeyboardMarkup inlineKeyboardNew = new(
                        new[]
                        {
                            // first row
                            new []
                            {
                                InlineKeyboardButton.WithCallbackData("<<", "<<"),
                                InlineKeyboardButton.WithCallbackData("1.1", "11"),
                                InlineKeyboardButton.WithCallbackData("1.2", "12"),
                                InlineKeyboardButton.WithCallbackData(">>", ">>"),
                            }
                        });
                    _bot.EditMessageReplyMarkupAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId, inlineKeyboardNew);
                }
            }
            
        }

        private async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery)
        {
            if (callbackQuery.Data is not null)
            {
                var callbackData = JsonSerializer.Deserialize<TelegramNotificationSenderService.CallbackData>(callbackQuery.Data);

                if (callbackData is not null)
                {

                    if (callbackData.NewReminder == 0)
                    {
                        _eventRepository.RemoveEvent(callbackData.EventId);
                        await _bot.AnswerCallbackQueryAsync(callbackQueryId: callbackQuery.Id, text: "Event was taken");
                    }
                    else
                    {
                        var chEvent = _eventRepository.GetEvent(callbackData.EventId);
                        if (chEvent is not null)
                        {
                            chEvent.Reminder = callbackData.NewReminder;
                            chEvent.Notified = false;
                            _eventRepository.ChangeEvent(callbackData.EventId, chEvent);
                            await _bot.AnswerCallbackQueryAsync(callbackQueryId: callbackQuery.Id, text: "Event was postponed");
                        }
                    }
                }
            }
            if (callbackQuery.Message is not null)
                await _bot.DeleteMessageAsync(callbackQuery.From.Id, callbackQuery.Message.MessageId);
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

        private async Task BotOnInlineQueryReceived(InlineQuery inlineQuery)
        {
            _logger.LogInformation("Received inline query from: {inlineQueryFromId}", inlineQuery.From.Id);

            InlineQueryResult[] results = {
            // displayed result
            new InlineQueryResultArticle(
                id: "3",
                title: "TgBots",
                inputMessageContent: new InputTextMessageContent(
                    "hello"
                )
            )
        };

            await _bot.AnswerInlineQueryAsync(inlineQueryId: inlineQuery.Id,
                                                    results: results,
                                                    isPersonal: true,
                                                    cacheTime: 0);
        }

    }
}
