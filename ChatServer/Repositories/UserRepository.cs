﻿using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChatServer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private ConcurrentBag<User> _users = new();
        public ConcurrentBag<User> Users {
            get => _users;  
            set => _users = value;  
        }
        
        public UserRepository(IConfiguration config = null)
        {
            if (config != null)
            {
                _usersFileName = config.GetValue<string>("usersfilepath");
            }
        }
        private readonly string _usersFileName = "users.json";
        public async Task ReadAsync()
        {
            if (File.Exists(_usersFileName))
            {
                using FileStream stream = File.Open(_usersFileName, FileMode.Open);
                _users = new ConcurrentBag<User>(await JsonSerializer.DeserializeAsync<List<User>>(stream));
                await stream.DisposeAsync();
            }

        }
        public async Task WriteAsync()
        {

            using FileStream streamMessage = File.Create(_usersFileName);
            await JsonSerializer.SerializeAsync<ConcurrentBag<User>>(streamMessage, _users, new JsonSerializerOptions { WriteIndented = true });
            await streamMessage.DisposeAsync();


        }
        public void AddUser(string nameUser) => _users.Add(new User(nameUser, nameUser.GetHashCode()));

        public bool IsUserExist(string userName) => _users.Count(x => x.Name == userName) == 0;
    }
}
