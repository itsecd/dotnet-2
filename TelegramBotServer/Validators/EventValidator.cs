using System;
using TelegramBotServer.Model;

namespace TelegramBotServer.Validators
{
    public class EventValidator
    {
        public bool Validate(Event someEvent)
        {
            if (someEvent.Deadline < DateTime.Now)
                return false;

            return true;
        }
    }
}
