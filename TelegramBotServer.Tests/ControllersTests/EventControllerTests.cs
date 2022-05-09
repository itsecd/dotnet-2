using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TelegramBotServer.Controllers;
using TelegramBotServer.Model;
using TelegramBotServer.Repository;
using Xunit;

namespace TelegramBotServer.Tests.ControllersTests
{
    public class EventControllerTests
    {
        const int eventId = 5;
        const int subId = 9;
        const bool notified = false;
        const int unreachableReturnStatus = 404;
        const int reachableReturnStatus = 200;
        readonly DateTime validDeadline;

        public EventControllerTests()
        {
            validDeadline = DateTime.Now + TimeSpan.FromMinutes(30);
        }

        [Fact]
        public void GetReachiableEvent()
        {
            var mockRepo = new Mock<IEventRepository>();
            var someEvent = new Event
            {
                Id = eventId,
                SubscriberId = subId,
                Notified = notified,
                Deadline = validDeadline
            };
            mockRepo.Setup(r => r.GetEvent(eventId)).Returns(someEvent);
            var controller = new EventController(mockRepo.Object);

            var result = controller.Get(eventId).Value;

            Assert.Equal(result, someEvent);
        }

        [Fact]
        public void GetUnreachiableEvent()
        {
            var mockRepo = new Mock<IEventRepository>();
            mockRepo.Setup(r => r.GetEvent(eventId)).Returns((Event)null);
            var controller = new EventController(mockRepo.Object);

            var result = controller.Get(eventId).Result as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(unreachableReturnStatus, result.StatusCode);
        }

        [Fact]
        public void PutReachableEvent()
        {
            var mockRepo = new Mock<IEventRepository>();
            var someEvent = new Event
            {
                Id = eventId,
                SubscriberId = subId,
                Notified = notified,
                Deadline = validDeadline
            };
            mockRepo.Setup(r => r.ChangeEvent(eventId, someEvent)).Returns(true);
            var controller = new EventController(mockRepo.Object);

            var result = controller.Put(eventId, someEvent) as OkResult;

            Assert.NotNull(result);
            Assert.Equal(reachableReturnStatus, result.StatusCode);
        }

        [Fact]
        public void PutUnreachableEvent()
        {
            var mockRepo = new Mock<IEventRepository>();
            var someEvent = new Event
            {
                Id = eventId,
                SubscriberId = subId,
                Notified = notified,
                Deadline = validDeadline
            };
            mockRepo.Setup(r => r.ChangeEvent(eventId, someEvent)).Returns(false);
            var controller = new EventController(mockRepo.Object);

            var result = controller.Put(eventId, someEvent) as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(unreachableReturnStatus, result.StatusCode);
        }

        [Fact]
        public void DeleteUnreachableEvent()
        {
            var mockRepo = new Mock<IEventRepository>();
            mockRepo.Setup(r => r.RemoveEvent(eventId)).Returns(false);
            var controller = new EventController(mockRepo.Object);

            var result = controller.Delete(eventId) as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(unreachableReturnStatus, result.StatusCode);
        }

        [Fact]
        public void DeleteReachableEvent()
        {
            var mockRepo = new Mock<IEventRepository>();
            mockRepo.Setup(r => r.RemoveEvent(eventId)).Returns(true);
            var controller = new EventController(mockRepo.Object);

            var result = controller.Delete(eventId) as OkResult;

            Assert.NotNull(result);
            Assert.Equal(reachableReturnStatus, result.StatusCode);
        }
    }
}
