using System;
using Xunit;
using TelegramBot.Repository;
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
            user1.EventReminders.Add(
                new EventReminder(0, "TestReminder1", "TestReminder1Description", new DateTime(2022, 5, 23, 21, 0, 0), new TimeSpan(0)));
            user2.EventReminders.Add(
                new EventReminder(0, "TestReminder2", "TestReminder2Description", new DateTime(2022, 5, 23, 21, 0, 0), new TimeSpan(0)));
            user3.EventReminders.Add(
                new EventReminder(0, "TestReminder3", "TestReminder3Description", new DateTime(2022, 5, 21, 21, 0, 0), new TimeSpan(0)));
            var testUsersRepository = new UsersRepository();
            testUsersRepository.AddUser(user1);
            testUsersRepository.AddUser(user2);
            testUsersRepository.AddUser(user3);
            return testUsersRepository;
        }

        private static User CreateTestUser()
        {
            User user = new User("User1", 1, 111);
            user.EventReminders.Add(
                new EventReminder(0, "TestReminder1", "TestReminder1Description", new DateTime(2022, 5, 23, 21, 0, 0), new TimeSpan(0)));
            return user;
        }

        [Fact]
        public void WriteToFileTest()
        {
            var usersRepository = CreateTestRepository();
            usersRepository.WriteToFile();
            Assert.True(File.Exists("Users.json"));
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
        public void AddEventReminderTest()
        {
            var usersRepository = CreateTestRepository();
            usersRepository.AddEventReminder(1, 
                new EventReminder(0, "TestReminder", "TestReminderDescription", new DateTime(2022, 5, 23, 21, 0, 0), new TimeSpan(0)));
            Assert.True(usersRepository.FindUser(1).EventReminders.Count == 2);
        }

        [Fact]
        public void ChangeEventReminderTest()
        {
            var usersRepository = CreateTestRepository();
            usersRepository.ChangeEventReminder(1, 0,
                new EventReminder(0, "Successfull test", "ReminderDescription", new DateTime(2022, 5, 23, 21, 0, 0), new TimeSpan(0)));
            Assert.True(usersRepository.FindUser(1).EventReminders[0].Name == "Successfull test");
        }

        [Fact]
        public void RemoveEventReminderTest()
        {
            var usersRepository = CreateTestRepository();
            usersRepository.RemoveEventReminder(1, 0);
            Assert.True(usersRepository.FindUser(1).EventReminders.Count == 0);
        }
    }
}
