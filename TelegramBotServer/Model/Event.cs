using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TelegramBotServer.Model
{
    public class Event
    {
        public int Id { get; set; }
        public Subscriber Subscriber { get; set; }
        public DateTime Deadline { get; set; }
    }
}
