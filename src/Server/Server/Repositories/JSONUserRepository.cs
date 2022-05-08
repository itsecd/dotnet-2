using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Server.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Server.Repositories
{
    public class JSONUserRepository : IJSONUserRepository
    {
        public List<User> Users { get; set; } = new();
        private readonly string _storageFileName;

        public JSONUserRepository(IConfiguration configuration)
        {
            _storageFileName = configuration.GetSection("Files").Get<FileConfiguration>().FileOfUsers;
        }
        public void AddUser(User user)
        {
            user.Id = Users.Max(us => us.Id) + 1;
            Users.Add(user);
        }

        public void DeleteUsers(int id)
        {
            Users.Remove(Users.Single(user => user.Id == id));
        }

        public void DeleteAllUsers()
        {
            Users.Clear();
        }
        public IEnumerable<User> GetUsers()
        {
            return Users;
        }
        public User GetUser(int id)
        {
            return Users.Exists(user => user.Id == id) ? Users.Single(user => user.Id == id) : null;
        }

        public void LoadData()
        {
            if (!File.Exists(_storageFileName) || new FileInfo(_storageFileName).Length == 0)
            {
                Users = new List<User>();
                return;
            }
            using var reader = new StreamReader(_storageFileName);
            var jsonString = reader.ReadToEnd();
            Users = JsonConvert.DeserializeObject<List<User>>(jsonString);
        }

        public void SaveData()
        {
            var jsonString = JsonConvert.SerializeObject(Users);
            using var writer = new StreamWriter(_storageFileName);
            writer.Write(jsonString);
        }

        public void UpdateUser(int id, User user)
        {
            if (Users.Exists(user => user.Id == id))
            {
                var userFromRepo = Users.Single(us => us.Id == id);
                userFromRepo.Name = user.Name;
                userFromRepo.ChatId = user.ChatId;
                userFromRepo.Toggle = user.Toggle;
            }
        }
    }
}
