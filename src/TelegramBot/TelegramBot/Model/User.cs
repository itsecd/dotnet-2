using System.Collections.Generic;

namespace TelegramBot.Model
{
    public class User
    {
        public string Name { get; set; }
        public long UserId { get; set; }
        public long ChatId { get; set; }
        public List<EventReminder> EventReminders { get; set; }
    }
}
