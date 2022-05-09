using Telegram.Bot;
using Telegram.Bot.Types;

namespace TgBot.Services
{
    public class HandleNotifyService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<HandleNotifyService> _logger;

        public HandleNotifyService(ITelegramBotClient botClient, ILogger<HandleNotifyService> logger)
        {
            _botClient = botClient;
            _logger = logger;
        }
        public async Task<Message> EchoAsync(Server.Model.UserEvent userEvent)
        {
            var chat = await _botClient.GetChatAsync(userEvent.User.ChatId);
            _logger.LogInformation("EVENT OCCURED");
            string content = "ALERT\n" +
                $"I remind you about the event {userEvent.EventName}\n" +
                $"Time - {userEvent.DateNTime.Hour}:{userEvent.DateNTime.Minute}";
            return await _botClient.SendTextMessageAsync(chatId: chat.Id,
                                            text: content);
        }
    }
}
