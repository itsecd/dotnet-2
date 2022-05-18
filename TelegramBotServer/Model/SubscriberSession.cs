﻿using System;
using System.Collections.Generic;

namespace TelegramBotServer.Model
{
    public class SubscriberSession
    {
        public enum ChoiceType
        {
            Month,
            Day,
            Hour,
            Message
        }
        public ChoiceType? CurrentChoice { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? ViewedTime { get; set; }
        public Dictionary<int, int> NotificatedEventId { get; set; } = new Dictionary<int, int>();
    }
}
