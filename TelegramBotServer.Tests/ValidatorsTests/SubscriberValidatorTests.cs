using System.Collections.Generic;
using System.Linq;
using TelegramBotServer.Model;
using TelegramBotServer.Validators;
using Xunit;

namespace TelegramBotServer.Tests.ValidatorsTests
{
    public class SubscriberValidatorTests
    {
        const int subId = 5;
        const int chatId = 9;
        readonly int[] eventsId = { 1, 2, 3 };

        [Fact]
        public void ValidateValidSub()
        {
            var validSub = new Subscriber
            {
                Id = subId,
                UserId = chatId,
                ChatId = chatId,
                EventsId = eventsId.ToList()
            };
            var validator = new SubscriberValidator(GetEvents());

            var result = validator.Validate(validSub);

            Assert.True(result);
        }

        [Fact]
        public void ValidateInvalidSub()
        {
            var validSub = new Subscriber
            {
                Id = subId,
                UserId = chatId,
                ChatId = chatId,
                EventsId = eventsId.ToList()
            };
            var validator = new SubscriberValidator(new List<Event>());

            var result = validator.Validate(validSub);

            Assert.False(result);
        }

        List<Event> GetEvents()
        {
            var events = new List<Event>();

            foreach (var eventId in eventsId)
            {
                events.Add(new Event { Id = eventId });
            }
            return events;
        }
    }
}
