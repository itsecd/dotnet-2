using System;
using System.Collections.Generic;
using Xunit;
using Lab2Server.Controllers;
using Lab2Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab2ServerTests.Controllers
{
    public class UserControllerTests
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

        private static UserController CreateTestController()
        {
            var TestController = new UserController(CreateTestRepository());
            return TestController;
        }
        [Fact()]
        public void GetUserTest()
        {
            var controller = CreateTestController();
            Assert.True(controller.Get(1).Value.UserId == 1);

        }

        [Fact()]
        public void PostReminderTest()
        {
            var controller = CreateTestController();
            controller.Post(1, new Reminder { Id = 2, DateTime = new DateTime(2021, 5, 21, 19, 18, 10), Name = "Reminder1", Description = "Description1", RepeatPeriod = 1 });
            Assert.True(controller.Get(1).Value.ReminderList.Exists(x => x.Id == 2));
        }

        [Fact()]
        public void PutUserTest()
        {
            var controller = CreateTestController();
            controller.Put(1, "NewName");
            Assert.True(controller.Get(1).Value.UserName == "NewName");
        }

        [Fact()]
        public void PutReminderTest()
        {
            var controller = CreateTestController();
            controller.Put(1, 1, new Reminder { Id = 1, DateTime = new DateTime(2021, 5, 21, 19, 18, 10), Name = "NewReminder", Description = "Description1", RepeatPeriod = 1 });
            Assert.True(controller.Get(1).Value.ReminderList.Find(x => x.Name == "NewReminder").Id == 1);
        }

        [Fact()]
        public void DeleteUserTest()
        {
            var controller = CreateTestController();
            controller.Delete(1);
            var actionResult = controller.Get(1);
            var result = actionResult.Result as NotFoundObjectResult;
            Assert.Null(result);
        }

        [Fact()]
        public void DeleteReminderTest()
        {
            var controller = CreateTestController();
            controller.Delete(1, 1);
            Assert.Null(controller.Get(1).Value.ReminderList.Find(x => x.Id == 1));
        }
    }
}
