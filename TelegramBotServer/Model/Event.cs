using System;
using System.ComponentModel.DataAnnotations;

namespace TelegramBotServer.Model
{
    public class Event
    {
        [Range(1, int.MaxValue, ErrorMessage = "Id should be greater than or equal to 1")]
        public int Id { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "SubscriberId should be greater than or equal to 1")]
        public int SubscriberId { get; set; }
        public DateTime Deadline { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Reminder should be greater than 0")]
        public int Reminder { get; set; }
        public bool Notified { get; set; }
    }
}
