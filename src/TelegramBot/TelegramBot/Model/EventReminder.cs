using System;

namespace TelegramBot.Model
{
    public class EventReminder
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Time { get; set; }
        public int RepeatPeriod { get; set; }
    }
}
