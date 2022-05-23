using System;

namespace TelegramBot.Model
{
    public class EventReminder
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Time { get; set; }
        public TimeSpan RepeatPeriod { get; set; }

        public EventReminder() {}

        public EventReminder(long id, string name, string description, DateTime time, TimeSpan repeatPeriod)
        {
            Id = id;
            Name = name;
            Description = description;
            Time = time;
            RepeatPeriod = repeatPeriod;
        }
    }
}
