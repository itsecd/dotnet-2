using System;

namespace TelegramBotClient.Model
{
    public  class EventReminder
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Time { get; set; }
        public TimeSpan RepeatPeriod { get; set; }
    }
}
