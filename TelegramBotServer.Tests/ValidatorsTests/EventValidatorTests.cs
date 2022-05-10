using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotServer.Model;
using TelegramBotServer.Validators;
using Xunit;

namespace TelegramBotServer.Tests.ValidatorsTests
{
    public class EventValidatorTests
    {
        const int eventId = 5;
        const int subId = 9;
        const bool notified = false;
        readonly DateTime validDeadline;
        readonly DateTime invalidDeadline;

        public EventValidatorTests()
        {
            validDeadline = DateTime.Now + TimeSpan.FromMinutes(30);
            invalidDeadline = DateTime.Now - TimeSpan.FromMinutes(30);
        }

        [Fact]
        public void ValideValidEvent()
        {
            var validEvent = new Event
            {
                Id = eventId,
                SubscriberId = subId,
                Notified = notified,
                Deadline = validDeadline
            };
            var validator = new EventValidator();

            var result = validator.Validate(validEvent);

            Assert.True(result);
        }

        [Fact]
        public void ValideInvalidEvent()
        {
            var invalidEvent = new Event
            {
                Id = eventId,
                SubscriberId = subId,
                Notified = notified,
                Deadline = invalidDeadline
            };
            var validator = new EventValidator();

            var result = validator.Validate(invalidEvent);

            Assert.False(result);
        }
    }
}
