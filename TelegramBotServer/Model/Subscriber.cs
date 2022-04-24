using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TelegramBotServer.Model
{
    public class Subscriber
    {
        public int Id { get; set; }
        public long ChatId { get; set; }
        public List<int> EventsId { get; set; }
    }
}
