using System;

namespace Lab2Server.Models
{
    public class Reminder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public int RepeatPeriod { get; set; }
    }
}
