using Server.Model;
using Server.Repositories;
using System.Collections.Generic;
using Xunit;

namespace TestServer
{
    public class UserRepositoryTest
    {
        private static User CreateUser(string name, long chatId)
        {
            return new User()
            {
                Name = name,
                ChatId = chatId,
                Toggle = true
            };
        }

        [Fact]
        public void GetUserTest()
        {
            var userRepository = new JSONUserRepository();
            var user = CreateUser("puti_love", 12312451);
            user.Id = 1;
            userRepository.AddUser(user);
            var returnedUser = userRepository.GetUser(1);
            Assert.Equal(user, returnedUser);
            userRepository.DeleteUser(1);
        }

        [Fact]
        public void GetUsersTest()
        {
            var userRepository = new JSONUserRepository();
            var user1 = CreateUser("puti_love", 12312451);
            var user2 = CreateUser("puti_love2", 12315125);
            var users = new List<User>{ user1, user2 };
            userRepository.AddUser(user1);
            userRepository.AddUser(user2);
            Assert.Equal(users, userRepository.GetUsers());
            userRepository.DeleteAllUsers();
        }

        [Fact]
        public void AddUserTest()
        {
            var userRepository = new JSONUserRepository();
            var user = CreateUser("puti_love", 12312451);
            userRepository.AddUser(user);
            Assert.Equal(user, userRepository.GetUser(1));
            userRepository.DeleteUser(1);
        }

        [Fact]
        public void UpdateUserTest()
        {
            var userRepository = new JSONUserRepository();
            var user = CreateUser("puti_love", 12312451);
            userRepository.AddUser(user);
            Assert.Equal(user, userRepository.GetUser(1));
            var user1 = CreateUser("puti_love1", 1111111);
            userRepository.UpdateUser(1, user1);
            Assert.Equal(user1, userRepository.GetUser(1));
            userRepository.DeleteUser(1);
        }

        [Fact]
        public void DeleteUserTest()
        {
            var userRepository = new JSONUserRepository();
            var user = CreateUser("puti_love", 12312451);
            user.Id = 1;
            userRepository.AddUser(user);
            Assert.Equal(user, userRepository.GetUser(1));
            var users = (List<User>)userRepository.GetUsers();
            userRepository.DeleteUser(1);
            Assert.Empty(users);
        }

        [Fact]
        public void DeleteUsersTest()
        {
            var userRepository = new JSONUserRepository();
            var user1 = CreateUser("puti_love", 12312451);
            var user2 = CreateUser("puti_love2", 12315125);
            userRepository.AddUser(user1);
            userRepository.AddUser(user2);
            userRepository.DeleteAllUsers();
            Assert.Empty(userRepository.GetUsers());
        }
    }
}