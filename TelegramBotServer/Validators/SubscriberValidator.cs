using System.Collections.Generic;
using System.Linq;
using TelegramBotServer.Model;

namespace TelegramBotServer.Validators
{
    public class SubscriberValidator
    {
        private IEnumerable<Event> _events;

        public SubscriberValidator(IEnumerable<Event> events)
        {
            _events = events;
        }

        public bool Validate(Subscriber subscriber)
        {
            if (subscriber.EventsId is not null)
            {
                foreach (var eventId in subscriber.EventsId)
                {
                    if (!_events.Any(e => e.Id == eventId))
                        return false;
                }
            }
            return true;
        }
    }
}
