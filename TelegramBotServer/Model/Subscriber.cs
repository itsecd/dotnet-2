using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TelegramBotServer.Model
{
    public class Subscriber
    {
        [Range(0, int.MaxValue, ErrorMessage = "Id should be positive number")]
        public int Id { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = "UserId should be greater than or equal to 1")]
        public long UserId { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = "ChatId should be greater than or equal to 1")]
        public long ChatId { get; set; }
        public List<int>? EventsId { get; set; }
    }
}
