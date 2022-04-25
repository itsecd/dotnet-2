using System.Collections.Generic;

namespace TelegramBotServer.Model
{
    public class Subscriber
    {
        public long Id { get; set; }
        public long ChatId { get; set; }
        public List<int> EventsId { get; set; }
    }
}
