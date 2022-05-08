using System.Collections.Generic;

namespace TelegramBotServer.Model
{
    public class Subscriber
    {
        public int Id { get; set; }

        public long UserId { get; set; }
        public long ChatId { get; set; }
        public List<int>? EventsId { get; set; }
    }
}
