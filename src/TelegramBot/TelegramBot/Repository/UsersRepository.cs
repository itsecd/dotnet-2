using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using TelegramBot.Model;

namespace TelegramBot.Repository
{
    public class UsersRepository : IUsersRepository
    {
        private List<User> _users;
        private readonly string _storageFileName;

        public UsersRepository(IConfiguration configuration = null)
        {
            if (configuration != null)
            {
                _storageFileName = configuration.GetValue<string>("RepositoryFilePath");
            }
            else
            {
                _storageFileName = "Users.json";
            }
            _users = new List<User>();
        }
        public void ReadFromFile()
        {
            if (!File.Exists(_storageFileName))
            {
                _users = new List<User>();
                return;
            }
            var jsonString = File.ReadAllText(_storageFileName);
            _users = JsonSerializer.Deserialize<List<User>>(jsonString);
        }

        public void WriteToFile()
        {
            var jsonString = JsonSerializer.Serialize<List<User>>(_users);
            File.WriteAllText(_storageFileName, jsonString);
        }

        public List<User> GetUsers()
        {
            return _users;
        }

        public bool IsUserExist(User user)
        {
            if (_users.Exists(x => x.UserId == user.UserId))
            {
                return true;
            }
            return false;
        }

        public User FindUser(long userId)
        {
            return _users.Find(x => x.UserId == userId);
        }

        public void AddUser(User user)
        {
            if (IsUserExist(user)) return;

            _users.Add(user);
        }

        public void RemoveUser(long userId)
        {
            _users.RemoveAll(x => x.UserId == userId);
        }

        public void AddEventReminder(long userId, EventReminder reminder)
        {
            var user = FindUser(userId);
            if (user.EventReminders.Count == 0)
            {
                reminder.Id = 0;
            }
            else
            {
                reminder.Id = user.EventReminders.Max(x => x.Id) + 1;
            }
            user.EventReminders.Add(reminder);
        }

        public void ChangeEventReminder(long userId, long id, EventReminder reminder)
        {
            var user = FindUser(userId);
            var reminderId = user.EventReminders.FindIndex(x => x.Id == id);
            user.EventReminders[reminderId] = reminder;
        }

        public void RemoveEventReminder(long userId, long id)
        {
            var user = FindUser(userId);
            user.EventReminders.RemoveAll(x => x.Id == id);
        }
    }
}
