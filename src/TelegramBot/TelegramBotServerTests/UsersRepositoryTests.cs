using System;
using Xunit;
using TelegramBot.Repository;
using TelegramBot.Model;

namespace TelegramBotServerTests
{
    public class UsersRepositoryTests
    {
        private static UsersRepository CreateTestRepository()
        {
            User user1 = new User("User1", 1, 111);
            User user2 = new User("User2", 2, 222);
            User user3 = new User("User3", 3, 333);
        }

        [Fact]
        public void Test1()
        {

        }
    }
}
