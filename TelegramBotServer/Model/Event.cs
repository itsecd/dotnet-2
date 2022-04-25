using System;

namespace TelegramBotServer.Model
{
    public class Event
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public DateTime Deadline { get; set; }
        public int Reminder { get; set; }
    }
}
