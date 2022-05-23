using System.Collections.Generic;

namespace TelegramBot.Model
{
    public class User
    {
        public string Name { get; set; }
        public long UserId { get; set; }
        public long ChatId { get; set; }
        public List<EventReminder> EventReminders { get; set; }

        public User() { }

        public User(string name, long userId, long chatId)
        {
            Name = name;
            UserId = userId;
            ChatId = chatId;
            EventReminders = new List<EventReminder>();
        }
    }
}
