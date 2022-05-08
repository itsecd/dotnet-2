using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelegramBotServer.Model;

namespace TelegramBotServer.Validators
{
    public class EventValidator
    {
        private IEnumerable<Event> _events;

        public EventValidator(IEnumerable<Event> events)
        {
            _events = events;
        }

        public bool Validate(Event someEvent)
        {
            if (someEvent.Deadline < DateTime.Now)
                return false;

            return true;
        }
    }
}
