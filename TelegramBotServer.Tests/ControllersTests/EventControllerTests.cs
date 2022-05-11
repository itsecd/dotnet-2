using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using TelegramBotServer.Controllers;
using TelegramBotServer.Model;
using TelegramBotServer.Repository;
using Xunit;

namespace TelegramBotServer.Tests.ControllersTests
{
    public class EventControllerTests
    {
        private const int EventId = 5;
        private const int SubId = 9;
        private const bool Notified = false;
        private const int UnreachableReturnStatus = 404;
        private const int ReachableReturnStatus = 200;
        private readonly DateTime validDeadline;

        public EventControllerTests()
        {
            validDeadline = DateTime.Now + TimeSpan.FromMinutes(30);
        }

        [Fact]
        public void GetReachableEvent()
        {
            var mockRepo = new Mock<IEventRepository>();
            var someEvent = new Event
            {
                Id = EventId,
                SubscriberId = SubId,
                Notified = Notified,
                Deadline = validDeadline
            };
            mockRepo.Setup(r => r.GetEvent(EventId)).Returns(someEvent);
            var controller = new EventController(mockRepo.Object);

            var result = controller.Get(EventId).Value;

            Assert.Equal(result, someEvent);
        }

        [Fact]
        public void GetUnreachableEvent()
        {
            var mockRepo = new Mock<IEventRepository>();
            mockRepo.Setup(r => r.GetEvent(EventId)).Returns((Event)null);
            var controller = new EventController(mockRepo.Object);

            var result = controller.Get(EventId).Result as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(UnreachableReturnStatus, result.StatusCode);
        }

        [Fact]
        public void PutReachableEvent()
        {
            var mockRepo = new Mock<IEventRepository>();
            var someEvent = new Event
            {
                Id = EventId,
                SubscriberId = SubId,
                Notified = Notified,
                Deadline = validDeadline
            };
            mockRepo.Setup(r => r.ChangeEvent(EventId, someEvent)).Returns(true);
            var controller = new EventController(mockRepo.Object);

            var result = controller.Put(EventId, someEvent) as OkResult;

            Assert.NotNull(result);
            Assert.Equal(ReachableReturnStatus, result.StatusCode);
        }

        [Fact]
        public void PutUnreachableEvent()
        {
            var mockRepo = new Mock<IEventRepository>();
            var someEvent = new Event
            {
                Id = EventId,
                SubscriberId = SubId,
                Notified = Notified,
                Deadline = validDeadline
            };
            mockRepo.Setup(r => r.ChangeEvent(EventId, someEvent)).Returns(false);
            var controller = new EventController(mockRepo.Object);

            var result = controller.Put(EventId, someEvent) as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(UnreachableReturnStatus, result.StatusCode);
        }

        [Fact]
        public void DeleteUnreachableEvent()
        {
            var mockRepo = new Mock<IEventRepository>();
            mockRepo.Setup(r => r.RemoveEvent(EventId)).Returns(false);
            var controller = new EventController(mockRepo.Object);

            var result = controller.Delete(EventId) as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(UnreachableReturnStatus, result.StatusCode);
        }

        [Fact]
        public void DeleteReachableEvent()
        {
            var mockRepo = new Mock<IEventRepository>();
            mockRepo.Setup(r => r.RemoveEvent(EventId)).Returns(true);
            var controller = new EventController(mockRepo.Object);

            var result = controller.Delete(EventId) as OkResult;

            Assert.NotNull(result);
            Assert.Equal(ReachableReturnStatus, result.StatusCode);
        }
    }
}
