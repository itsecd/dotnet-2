using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Linq;
using TelegramBotServer.Controllers;
using TelegramBotServer.Model;
using TelegramBotServer.Repository;
using Xunit;

namespace TelegramBotServer.Tests.ControllersTests
{
    public class SubscriberControllerTests
    {
        private const int SubId = 5;
        private const int ChatId = 9;
        private readonly int[] eventsId = { 1, 2, 3 };
        private const int UnreachableReturnStatus = 404;
        private const int ReachableReturnStatus = 200;

        [Fact]
        public void GetReachableSub()
        {
            var mockRepo = new Mock<ISubscriberRepository>();
            var subscriber = new Subscriber
            {
                Id = SubId,
                UserId = ChatId,
                ChatId = ChatId,
                EventsId = eventsId.ToList()
            };
            mockRepo.Setup(r => r.GetSubscriber(SubId)).Returns(subscriber);
            var controller = new SubscriberController(mockRepo.Object);

            var result = controller.Get(SubId).Value;

            Assert.Equal(result, subscriber);
        }

        [Fact]
        public void GetUnreachableSub()
        {
            var mockRepo = new Mock<ISubscriberRepository>();
            mockRepo.Setup(r => r.GetSubscriber(SubId)).Returns((Subscriber)null);
            var controller = new SubscriberController(mockRepo.Object);

            var result = controller.Get(SubId).Result as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(UnreachableReturnStatus, result.StatusCode);
        }

        [Fact]
        public void PutReachableSub()
        {
            var mockRepo = new Mock<ISubscriberRepository>();
            var subscriber = new Subscriber
            {
                Id = SubId,
                UserId = ChatId,
                ChatId = ChatId,
                EventsId = eventsId.ToList()
            };
            mockRepo.Setup(r => r.ChangeSubscriber(SubId, subscriber)).Returns(true);
            var controller = new SubscriberController(mockRepo.Object);

            var result = controller.Put(SubId, subscriber) as OkResult;

            Assert.NotNull(result);
            Assert.Equal(ReachableReturnStatus, result.StatusCode);
        }

        [Fact]
        public void PutUnreachableSub()
        {
            var mockRepo = new Mock<ISubscriberRepository>();
            var subscriber = new Subscriber
            {
                Id = SubId,
                UserId = ChatId,
                ChatId = ChatId,
                EventsId = eventsId.ToList()
            };
            mockRepo.Setup(r => r.ChangeSubscriber(SubId, subscriber)).Returns(false);
            var controller = new SubscriberController(mockRepo.Object);

            var result = controller.Put(SubId, subscriber) as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(UnreachableReturnStatus, result.StatusCode);
        }

        [Fact]
        public void DeleteUnreachableEvent()
        {
            var mockRepo = new Mock<IEventRepository>();
            mockRepo.Setup(r => r.RemoveEvent(SubId)).Returns(false);
            var controller = new EventController(mockRepo.Object);

            var result = controller.Delete(SubId) as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(UnreachableReturnStatus, result.StatusCode);
        }

        [Fact]
        public void DeleteReachableEvent()
        {
            var mockRepo = new Mock<IEventRepository>();
            mockRepo.Setup(r => r.RemoveEvent(SubId)).Returns(true);
            var controller = new EventController(mockRepo.Object);

            var result = controller.Delete(SubId) as OkResult;

            Assert.NotNull(result);
            Assert.Equal(ReachableReturnStatus, result.StatusCode);
        }
    }
}
