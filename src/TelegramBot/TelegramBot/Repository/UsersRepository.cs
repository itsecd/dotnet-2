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

        public UsersRepository(IConfiguration configuration)
        {
            _storageFileName = configuration.GetValue<string>("RepositoryFilePath");
        }
        private void ReadFromFile()
        {
            if (_users != null) return;

            if (!File.Exists(_storageFileName))
            {
                _users = new List<User>();
                return;
            }
            var jsonString = File.ReadAllText(_storageFileName);
            _users = JsonSerializer.Deserialize<List<User>>(jsonString);
        }

        private void WriteToFile()
        {
            var jsonString = JsonSerializer.Serialize<List<User>>(_users);
            File.WriteAllText(_storageFileName, jsonString);
        }

        public List<User> GetUsers()
        {
            ReadFromFile();
            return _users;
        }

        public bool IsUserExist(User user)
        {
            ReadFromFile();
            if (_users.Exists(x => x.UserId == user.UserId))
            {
                return true;
            }
            return false;
        }

        public User FindUser(long id)
        {
            ReadFromFile();
            return _users.Find(x => x.UserId == id);
        }

        public void AddUser(User user)
        {
            if (IsUserExist(user)) return;

            ReadFromFile();
            _users.Add(user);
            WriteToFile();
        }

        public void RemoveUser(long id)
        {
            ReadFromFile();
            _users.RemoveAll(x => x.UserId == id);
            WriteToFile();
        }

        public void AddEventReminder(long id, EventReminder reminder)
        {
            var user = FindUser(id);
            reminder.Id = user.EventReminders.Max(x => x.Id) + 1;
            user.EventReminders.Add(reminder);
            WriteToFile();
        }

        public void ChangeEventReminder(long userId, long id, EventReminder reminder)
        {
            var user = FindUser(id);
            var reminderId = user.EventReminders.FindIndex(x => x.Id == id);
            user.EventReminders[reminderId] = reminder;
            WriteToFile();
        }

        public void RemoveEventReminder(long userId, long id)
        {
            var user = FindUser(id);
            user.EventReminders.RemoveAll(x => x.Id == id);
            WriteToFile();
        }
    }
}
