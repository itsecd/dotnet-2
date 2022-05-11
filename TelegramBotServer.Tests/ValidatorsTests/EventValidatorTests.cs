using System;
using TelegramBotServer.Model;
using TelegramBotServer.Validators;
using Xunit;

namespace TelegramBotServer.Tests.ValidatorsTests
{
    public class EventValidatorTests
    {
        private const int EventId = 5;
        private const int SubId = 9;
        private const bool Notified = false;
        private readonly DateTime validDeadline;
        private readonly DateTime invalidDeadline;

        public EventValidatorTests()
        {
            validDeadline = DateTime.Now + TimeSpan.FromMinutes(30);
            invalidDeadline = DateTime.Now - TimeSpan.FromMinutes(30);
        }

        [Fact]
        public void ValidateValidEvent()
        {
            var validEvent = new Event
            {
                Id = EventId,
                SubscriberId = SubId,
                Notified = Notified,
                Deadline = validDeadline
            };
            var validator = new EventValidator();

            var result = validator.Validate(validEvent);

            Assert.True(result);
        }

        [Fact]
        public void ValidateInvalidEvent()
        {
            var invalidEvent = new Event
            {
                Id = EventId,
                SubscriberId = SubId,
                Notified = Notified,
                Deadline = invalidDeadline
            };
            var validator = new EventValidator();

            var result = validator.Validate(invalidEvent);

            Assert.False(result);
        }
    }
}
