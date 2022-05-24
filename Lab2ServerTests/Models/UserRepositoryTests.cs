using Xunit;
using Lab2Server.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Lab2ServerTests.Models
{
    public class UserRepositoryTests
    {
        private static UserRepository CreateTestRepository()
        {
            User user1 = new User { UserId = 1, UserName = "user1", ChatId = 11, ReminderList = new List<Reminder>() };
            user1.ReminderList.Add(new Reminder { Id = 1, DateTime = new DateTime(2021, 5, 21, 19, 18, 10), Name = "Reminder1", Description = "Description1", RepeatPeriod = 1 });
            User user2 = new User { UserId = 2, UserName = "user2", ChatId = 22, ReminderList = new List<Reminder>() };
            user1.ReminderList.Add(new Reminder { Id = 1, DateTime = new DateTime(2021, 5, 21, 19, 18, 10), Name = "Reminder1", Description = "Description1", RepeatPeriod = 1 });
            UserRepository usersRepository = new UserRepository();
            usersRepository.AddNewUser(user1);
            usersRepository.AddNewUser(user2);
            return usersRepository;
        }

        private static User CreateTestUser()
        {
            User user1 = new User { UserId = 1, UserName = "user1", ChatId = 11, ReminderList = new List<Reminder>() };
            user1.ReminderList.Add(new Reminder { Id = 3, DateTime = new DateTime(2021, 5, 21, 19, 18, 10), Name = "Reminder3", Description = "Description3", RepeatPeriod = 1 });
            return user1;
        }

        [Fact]
        public void WriteToFileTest()
        {
            var userRepository = CreateTestRepository();
            userRepository.WriteToFile();
            Assert.True(File.Exists("Users.xml"));
        }

        [Fact]
        public void ReadFromFileTest()
        {
            var userRepository = CreateTestRepository();
            userRepository.ReadFromFile();
            Assert.True(true);
        }

        [Fact]
        public void AddNewUserTest()
        {
            var user = CreateTestUser();
            var userRepository = CreateTestRepository();
            userRepository.AddNewUser(user);
            Assert.True(userRepository.FindUser((int)user.UserId).UserId == user.UserId);
        }

        [Fact]
        public void FindUserTest()
        {
            var userRepository = CreateTestRepository();
            Assert.True(userRepository.FindUser(1).UserId == 1);
        }

        [Fact]
        public void RemoveUserTest()
        {
            var userRepository = CreateTestRepository();
            userRepository.RemoveUser(1);
            Assert.True(userRepository.FindUser(1) == null);
        }

        [Fact]
        public void ChangeNameTest()
        {
            var userRepository = CreateTestRepository();
            userRepository.ChangeName(1, "NewName");
            Assert.True(userRepository.FindUser(1).UserName == "NewName");
        }

        [Fact]
        public void AddReminderTest()
        {
            var userRepository = CreateTestRepository();
            userRepository.AddReminder(1, new Reminder { Id = 2, DateTime = new DateTime(2021, 5, 21, 19, 18, 10), Name = "NewReminder", Description = "NewDescription", RepeatPeriod = 1 });
            Assert.True(userRepository.FindUser(1).ReminderList.Exists(x => x.Id == 2));
        }

        [Fact]
        public void ChangeReminderTest()
        {
            var userRepository = CreateTestRepository();
            userRepository.ChangeReminder(1, 1, new Reminder { Id = 1, DateTime = new DateTime(2021, 5, 21, 19, 18, 10), Name = "NewReminder", Description = "NewDescription", RepeatPeriod = 1 });
            Assert.True(userRepository.FindUser(1).ReminderList.Find(x => x.Id == 1).Name == "NewReminder");
        }

        [Fact]
        public void RemoveReminderTest()
        {
            var userRepository = CreateTestRepository();
            userRepository.RemoveReminder(1, 1);
            Assert.False(userRepository.FindUser(1).ReminderList.Exists(x => x.Id == 1));
        }
    }
}