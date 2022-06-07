using System;
using Xunit;
using TelegramBot.Repositories;
using TelegramBot.Model;
using System.IO;

namespace TelegramBotServerTests
{
    public class UsersRepositoryTests
    {
        private static UsersRepository CreateTestRepository()
        {
            User user1 = new User("User1", 1, 111);
            User user2 = new User("User2", 2, 222);
            User user3 = new User("User3", 3, 333);
            user1.Events.Add(new UserEvent(0, "TestEvent1", "TestEvent1Description", new DateTime(2022, 6, 6, 23, 0, 0), new TimeSpan(0)));
            user2.Events.Add(new UserEvent(0, "TestEvent2", "TestEvent2Description", new DateTime(2022, 6, 6, 23, 0, 0), new TimeSpan(0)));
            user3.Events.Add(new UserEvent(0, "TestEvent3", "TestEvent3Description", new DateTime(2022, 6, 6, 23, 0, 0), new TimeSpan(0)));
            var testUsersRepository = new UsersRepository();
            testUsersRepository.AddUser(user1);
            testUsersRepository.AddUser(user2);
            testUsersRepository.AddUser(user3);
            return testUsersRepository;
        }

        private static User CreateTestUser()
        {
            User user = new User("User1", 1, 111);
            user.Events.Add(new UserEvent(0, "TestEvent1", "TestEvent1Description", new DateTime(2022, 6, 6, 23, 0, 0), new TimeSpan(0)));
            return user;
        }

        [Fact]
        public void WriteToFileTest()
        {
            var usersRepository = CreateTestRepository();
            usersRepository.WriteToFile();
            Assert.True(File.Exists("Users.xml"));
        }

        [Fact]
        public void ReadFromFileTest()
        {
            var usersRepository = new UsersRepository();
            usersRepository.AddUser(CreateTestUser());
            usersRepository.WriteToFile();
            var testUsersRepository = new UsersRepository();
            testUsersRepository.ReadFromFile();
            Assert.True(testUsersRepository.GetUsers().Count != 0);
        }

        [Fact]
        public void AddUserTest()
        {
            var usersRepository = new UsersRepository();
            var user = CreateTestUser();
            usersRepository.AddUser(user);
            Assert.True(usersRepository.IsUserExist(user));
        }

        [Fact]
        public void RemoveUserTest()
        {
            var usersRepository = CreateTestRepository();
            var user = CreateTestUser();
            usersRepository.RemoveUser(1);
            Assert.True(!usersRepository.IsUserExist(user));
        }

        [Fact]
        public void FindUserTest()
        {
            var usersRepository = CreateTestRepository();
            Assert.True(usersRepository.FindUser(1).Name == "User1");
        }

        [Fact]
        public void AddEventTest()
        {
            var usersRepository = CreateTestRepository();
            usersRepository.AddEvent(1, new UserEvent(0, "TestEvent", "TestEventDescription", new DateTime(2022, 5, 23, 21, 0, 0), new TimeSpan(0)));
            Assert.True(usersRepository.FindUser(1).Events.Count == 2);
        }

        [Fact]
        public void ChangeEventTest()
        {
            var usersRepository = CreateTestRepository();
            usersRepository.ChangeEvent(1, 0, new UserEvent(0, "Successful test", "EventDescription", new DateTime(2022, 5, 23, 21, 0, 0), new TimeSpan(0)));
            Assert.True(usersRepository.FindUser(1).Events[0].Name == "Successful test");
        }

        [Fact]
        public void RemoveEventTest()
        {
            var usersRepository = CreateTestRepository();
            usersRepository.RemoveEvent(1, 0);
            Assert.True(usersRepository.FindUser(1).Events.Count == 0);
        }
    }
}