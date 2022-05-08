using Microsoft.Extensions.Configuration;
using Server.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Server.Repositories
{
    public class JSONUserRepository : IJSONUserRepository
    {
        public List<User> entyties { get; set; } = new();
        private readonly string _storageFileName;

        public JSONUserRepository(IConfiguration configuration)
        {
            _storageFileName = configuration.GetSection("Files").Get<FileConfiguration>().FileOfUsers;
        }
        public void AddUser(User user)
        {
            user.Id = entyties.Max(us => us.Id) + 1;
            entyties.Add(user);
        }

        public void DeleteUsers(int id)
        {
            entyties.Remove(entyties.Single(user => user.Id == id));
        }

        public void DeleteAllUsers()
        {
            entyties.Clear();
        }
        public IEnumerable<User> GetUsers()
        {
            return entyties;
        }
        public User GetUser(int id)
        {
            return entyties.Exists(user => user.Id == id) ? entyties.Single(user => user.Id == id) : null;
        }

        public void LoadData()
        {
            if (!File.Exists(_storageFileName) || new FileInfo(_storageFileName).Length == 0)
            {
                entyties = new List<User>();
                return;
            }
            using var reader = new StreamReader(_storageFileName);
            string jsonString = reader.ReadToEnd();
            entyties = JsonSerializer.Deserialize<List<User>>(jsonString);
        }

        public void SaveData()
        {
            string jsonString = JsonSerializer.Serialize(entyties);
            using var writer = new StreamWriter(_storageFileName);
            writer.Write(jsonString);
        }

        public void UpdateUser(int id, User user)
        {
            if (entyties.Exists(user => user.Id == id))
            {
                User us = entyties.Single(us => us.Id == id);
                us.Name = user.Name;
                us.ChatId = user.ChatId;
                us.Toggle = user.Toggle;
                SaveData();
            }
        }
    }
}
