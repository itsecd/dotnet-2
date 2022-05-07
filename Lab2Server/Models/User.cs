using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2Server.Models
{
    public class User
    {
        public string UserName { get; set; }
        public long UserId { get; set; }
        public long ChatId { get; set; }
        public List<Reminder> ReminderList { get; set; } = new List<Reminder>();
    }
}
