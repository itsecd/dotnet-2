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
        const int subId = 5;
        const int chatId = 9;
        readonly int[] eventsId = { 1, 2, 3 };
        const int unreachableReturnStatus = 404;
        const int reachableReturnStatus = 200;

        [Fact]
        public void GetReachiableSub()
        {
            var mockRepo = new Mock<ISubscriberRepository>();
            var subscriber = new Subscriber
            {
                Id = subId,
                UserId = chatId,
                ChatId = chatId,
                EventsId = eventsId.ToList()
            };
            mockRepo.Setup(r => r.GetSubscriber(subId)).Returns(subscriber);
            var controller = new SubscriberController(mockRepo.Object);

            var result = controller.Get(subId).Value;

            Assert.Equal(result, subscriber);
        }

        [Fact]
        public void GetUnreachiableSub()
        {
            var mockRepo = new Mock<ISubscriberRepository>();
            mockRepo.Setup(r => r.GetSubscriber(subId)).Returns((Subscriber)null);
            var controller = new SubscriberController(mockRepo.Object);

            var result = controller.Get(subId).Result as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(unreachableReturnStatus, result.StatusCode);
        }

        [Fact]
        public void PutReachableSub()
        {
            var mockRepo = new Mock<ISubscriberRepository>();
            var subscriber = new Subscriber
            {
                Id = subId,
                UserId = chatId,
                ChatId = chatId,
                EventsId = eventsId.ToList()
            };
            mockRepo.Setup(r => r.ChangeSubscriber(subId, subscriber)).Returns(true);
            var controller = new SubscriberController(mockRepo.Object);

            var result = controller.Put(subId, subscriber) as OkResult;

            Assert.NotNull(result);
            Assert.Equal(reachableReturnStatus, result.StatusCode);
        }

        [Fact]
        public void PutUnreachableSub()
        {
            var mockRepo = new Mock<ISubscriberRepository>();
            var subscriber = new Subscriber
            {
                Id = subId,
                UserId = chatId,
                ChatId = chatId,
                EventsId = eventsId.ToList()
            };
            mockRepo.Setup(r => r.ChangeSubscriber(subId, subscriber)).Returns(false);
            var controller = new SubscriberController(mockRepo.Object);

            var result = controller.Put(subId, subscriber) as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(unreachableReturnStatus, result.StatusCode);
        }

        [Fact]
        public void DeleteUnreachableEvent()
        {
            var mockRepo = new Mock<IEventRepository>();
            mockRepo.Setup(r => r.RemoveEvent(subId)).Returns(false);
            var controller = new EventController(mockRepo.Object);

            var result = controller.Delete(subId) as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(unreachableReturnStatus, result.StatusCode);
        }

        [Fact]
        public void DeleteReachableEvent()
        {
            var mockRepo = new Mock<IEventRepository>();
            mockRepo.Setup(r => r.RemoveEvent(subId)).Returns(true);
            var controller = new EventController(mockRepo.Object);

            var result = controller.Delete(subId) as OkResult;

            Assert.NotNull(result);
            Assert.Equal(reachableReturnStatus, result.StatusCode);
        }
    }
}
