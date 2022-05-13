using System.Collections.Generic;


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
