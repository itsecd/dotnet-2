using System;
using TelegramBotServer.Model;

namespace TelegramBotServer.Validators
{
    public class EventValidator
    {
        public bool Validate(Event someEvent) => someEvent.Deadline >= DateTime.Now;
    }
}
