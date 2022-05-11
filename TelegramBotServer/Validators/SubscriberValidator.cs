using System.Collections.Generic;
using System.Linq;
using TelegramBotServer.Model;

namespace TelegramBotServer.Validators
{
    public class SubscriberValidator
    {
        private readonly IEnumerable<Event> _events;

        public SubscriberValidator(IEnumerable<Event> events)
        {
            _events = events;
        }

        public bool Validate(Subscriber subscriber)
        {
            if (subscriber.EventsId is not null)
            {
                return subscriber.EventsId.All(eventId => _events.Any(e => e.Id == eventId));
            }
            return true;
        }
    }
}
