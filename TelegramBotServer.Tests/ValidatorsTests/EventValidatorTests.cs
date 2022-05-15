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
        private readonly DateTime _validDeadline;
        private readonly DateTime _invalidDeadline;

        public EventValidatorTests()
        {
            _validDeadline = DateTime.Now + TimeSpan.FromMinutes(30);
            _invalidDeadline = DateTime.Now - TimeSpan.FromMinutes(30);
        }

        [Fact]
        public void ValidateValidEvent()
        {
            var validEvent = new Event
            {
                Id = EventId,
                SubscriberId = SubId,
                Notified = Notified,
                Deadline = _validDeadline
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
                Deadline = _invalidDeadline
            };
            var validator = new EventValidator();

            var result = validator.Validate(invalidEvent);

            Assert.False(result);
        }
    }
}
