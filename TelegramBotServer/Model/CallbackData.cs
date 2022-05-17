using TelegramBotServer.Enums;

namespace TelegramBotServer.Model
{
    public class CallbackData
    {
        public CallbackDataType Type { get; set; }

        public string? Data { get; set; }
    }
}
