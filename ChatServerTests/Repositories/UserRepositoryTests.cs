using Xunit;
using System;
using System.Collections.Concurrent;
using System.IO;

namespace ChatServer.Repositories.Tests
{
    public class UserRepositoryTests
    {
        private static UserRepository CreateTestRepository()
        {
            var user1 = new User("user1", 1);
            var user2 = new User("user2", 2);
            var user3 = new User("user3", 3);

            return new UserRepository
            {
                Users = new ConcurrentBag<User> { user1, user2, user3 }
            };
        }

        [Fact()]
        public async void WriteAsyncTest()
        {
            var users = CreateTestRepository();
            await users.WriteAsync();
            Assert.True(File.Exists("users.json"));
        }

        [Fact()]
        public async void ReadAsyncTest()
        {
            var userRepositories = new UserRepository();
            await userRepositories.ReadAsync();
            var actual = userRepositories.Users.ToArray();
            Assert.True(Array.Exists(actual, x => x.Name == "user1"));

        }

        [Fact()]
        public void AddUserTest()
        {
            var userRepositories = CreateTestRepository();
            userRepositories.Users.Add(new User("user4", 1));
            var actual = userRepositories.Users.ToArray();
            Assert.True(Array.Exists(actual, x => x.Name == "user4"));
        }

        [Fact()]
        public void IsUserExistTest()
        {
            var userRepositories = CreateTestRepository();
            var actual = userRepositories.IsUserExist("user22222");
            Assert.True(actual);
        }
    }
}