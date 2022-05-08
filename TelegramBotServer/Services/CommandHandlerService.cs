using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotServer.Repository;

namespace TelegramBotServer.Services
{
    public class CommandHandlerService
    {
        private readonly ITelegramBotClient _bot;
        private readonly ISubscriberRepository _subscriberRepository;
        private readonly ILogger<CommandHandlerService> _logger;

        public CommandHandlerService(ITelegramBotClient bot, ILogger<CommandHandlerService> logger, ISubscriberRepository subscriberRepository)
        {
            _bot = bot;
            _logger = logger;
            _subscriberRepository = subscriberRepository;
        }

        public async Task ProcessUpdate(Update update)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => ProcessCommand(update.Message!),
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
                repository.AddSubscriber(new Model.Subscriber
                {
                    UserId = message.From.Id,
                    ChatId = message.Chat.Id,
                });
                return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                      text: "You successfully subscribed on notifications");
            }

            static async Task<Message> Unsubscribe(ITelegramBotClient bot, Message message, ISubscriberRepository repository)
            {
                var sub = repository.GetSubscribers().FirstOrDefault(s => s.UserId == message.From.Id);
                if (sub is null)
                    return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                      text: "You have not been subscribed to notifications");
                repository.RemoveSubscriber(sub.Id);
                return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                      text: "You successfully unsubscribed on notifications");
            }
        }

        private Task UnknownUpdateHandlerAsync(Update update)
        {
            _logger.LogInformation("Unknown update type: {updateType}", update.Type);
            return Task.CompletedTask;
        }

        public Task HandleErrorAsync(Exception exception)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogInformation("HandleError: {ErrorMessage}", ErrorMessage);
            return Task.CompletedTask;
        }

    }
}
