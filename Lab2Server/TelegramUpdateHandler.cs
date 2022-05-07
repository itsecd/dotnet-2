using Lab2Server.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Lab2Server
{
    public class TelegramUpdateHandler : IUpdateHandler
    {
        private IUserRepository _userRepository;
        public TelegramUpdateHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        async Task IUpdateHandler.HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Only process Message updates: https://core.telegram.org/bots/api#message
            if (update.Type != UpdateType.Message)
                return;
            // Only process text messages
            if (update.Message!.Type != MessageType.Text)
                return;

            var chatId = update.Message.Chat.Id; // 599338819
            var messageText = update.Message.Text;
            var username = update.Message.From.FirstName;
            var userId = update.Message.From.Id;
            _userRepository.AddNewUser(new Models.User()
            {
                ChatId = chatId,
                UserId = userId,
                UserName = username
            }
            );
            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

            // Echo received message text
            Message message = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Hello, World!",
                cancellationToken: cancellationToken);
        }
    }
}
