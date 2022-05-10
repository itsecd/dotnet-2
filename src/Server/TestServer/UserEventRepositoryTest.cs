using Server.Model;
using Server.Repositories;
using System;
using System.Collections.Generic;
using Xunit;

namespace TestServer
{
    public class UserEventRepositoryTest
    {
        private static UserEvent CreateUserEvent(int id, string eventName, DateTime dateTime, int eventFrequency)
        {
            var user = new User()
            {
                Id = 1,
                Name = "user",
                ChatId = 11111111,
                Toggle = true
            };
            return new UserEvent()
            {
                Id = id,
                User = user,
                EventName = eventName,
                DateNTime = dateTime,
                EventFrequency = eventFrequency
            };
        }

        [Fact]
        public void GetUserEventTest()
        {
            var userEventRepository = new JSONUserEventRepository();
            DateTime dateTime = DateTime.Now;
            var userEvent = CreateUserEvent(1, "testEvent", dateTime, 1);
            userEventRepository.AddUserEvent(userEvent);
            var returnedUser = userEventRepository.GetUserEvent(1);
            Assert.Equal(userEvent, returnedUser);
            userEventRepository.DeleteUserEvent(1);
        }

        [Fact]
        public void GetUsersTest()
        {
            DateTime dateTime = DateTime.Now;
            var userEventRepository = new JSONUserEventRepository();
            var userEvent1 = CreateUserEvent(1, "testEvent", dateTime, 1);
            var userEvent2 = CreateUserEvent(2, "testEvent2", dateTime, 2);
            var userEvents = new List<UserEvent>{ userEvent1, userEvent2 };
            userEventRepository.AddUserEvent(userEvent1);
            userEventRepository.AddUserEvent(userEvent2);
            Assert.Equal(userEvents, userEventRepository.GetUserEvents());
            userEventRepository.DeleteAllUserEvents();
        }

        [Fact]
        public void AddUserTest()
        {
            DateTime dateTime = DateTime.Now;
            var userEventRepository = new JSONUserEventRepository();
            var userEvent = CreateUserEvent(1, "testEvent", dateTime, 1);
            userEventRepository.AddUserEvent(userEvent);
            Assert.Equal(userEvent, userEventRepository.GetUserEvent(1));
            userEventRepository.DeleteUserEvent(1);
        }

        [Fact]
        public void UpdateUserTest()
        {
            DateTime dateTime = DateTime.Now;
            var userRepository = new JSONUserEventRepository();
            var userEvent = CreateUserEvent(1, "testEvent", dateTime, 1);
            userRepository.AddUserEvent(userEvent);
            Assert.Equal(userEvent, userRepository.GetUserEvent(1));
            DateTime dateTime2 = DateTime.Now;
            var userEvent1 = CreateUserEvent(2, "testEvent2", dateTime2, 2);
            userRepository.UpdateUserEvent(1, userEvent1);
            Assert.Equal(userEvent1, userRepository.GetUserEvent(1));
            userRepository.DeleteUserEvent(1);
        }

        [Fact]
        public void DeleteUserTest()
        {
            DateTime dateTime = DateTime.Now;
            var userEventRepository = new JSONUserEventRepository();
            var userEvent = CreateUserEvent(1, "testEvent", dateTime, 1);
            userEventRepository.AddUserEvent(userEvent);
            Assert.Equal(userEvent, userEventRepository.GetUserEvent(1));
            var userEvents = (List<UserEvent>)userEventRepository.GetUserEvents();
            userEventRepository.DeleteUserEvent(1);
            Assert.Empty(userEvents);
        }

        [Fact]
        public void DeleteUsersTest()
        {
            DateTime dateTime = DateTime.Now;
            var userEventRepository = new JSONUserEventRepository();
            var user1 = CreateUserEvent(1, "testEvent", dateTime, 1);
            var user2 = CreateUserEvent(2, "testEvent2", dateTime, 3);
            userEventRepository.AddUserEvent(user1);
            userEventRepository.AddUserEvent(user2);
            userEventRepository.DeleteAllUserEvents();
            Assert.Empty(userEventRepository.GetUserEvents());
        }
    }
}