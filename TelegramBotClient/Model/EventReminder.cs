using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotClient.Model
{
    public  class EventReminder
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime Time { get; set; }
        public TimeSpan RepeatPeriod { get; set; }
    }
}
