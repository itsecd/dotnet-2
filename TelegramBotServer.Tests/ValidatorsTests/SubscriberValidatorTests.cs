using System.Collections.Generic;
using System.Linq;
using TelegramBotServer.Model;
using TelegramBotServer.Validators;
using Xunit;

namespace TelegramBotServer.Tests.ValidatorsTests
{
    public class SubscriberValidatorTests
    {
        private const int SubId = 5;
        private const int ChatId = 9;
        private readonly int[] eventsId = { 1, 2, 3 };

        [Fact]
        public void ValidateValidSub()
        {
            var validSub = new Subscriber
            {
                Id = SubId,
                UserId = ChatId,
                ChatId = ChatId,
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
                Id = SubId,
                UserId = ChatId,
                ChatId = ChatId,
                EventsId = eventsId.ToList()
            };
            var validator = new SubscriberValidator(new List<Event>());

            var result = validator.Validate(validSub);

            Assert.False(result);
        }

        IEnumerable<Event> GetEvents() => from eventId in eventsId
                                          select (new Event { Id = eventId });
    }
}
