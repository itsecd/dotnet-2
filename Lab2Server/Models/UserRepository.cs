using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Extensions.Configuration;

namespace Lab2Server.Models
{
    public class UserRepository : IUserRepository
    {
        public UserRepository(IConfiguration configuration = null)
        {
            if (configuration != null)
            {
                _storageFileName = configuration.GetValue<string>("RepositoryFilePath");
            }
        }

        private readonly string _storageFileName = "users.xml";

        private List<User> _users = new();
        
        public void ReadFromFile()
        {
            if (_users != null) return;

            if (!File.Exists(_storageFileName))
            {
                _users = new List<User>();
                return;
            }
            var xmlSerializer = new XmlSerializer(typeof(List<User>));
            using var fileStream = new FileStream(_storageFileName, FileMode.Open);
            _users = (List<User>)xmlSerializer.Deserialize(fileStream);
        }

        public void WriteToFile()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<User>));
            using var fileStream = new FileStream(_storageFileName, FileMode.Create);
            xmlSerializer.Serialize(fileStream, _users);
        }

        public void AddNewUser(User newUser)
        {
            if (newUser == null)
                throw new ArgumentNullException(nameof(newUser));
            if (ExistUser(newUser))
            {
                return;
            }
            _users.Add(newUser);
        }
        private bool ExistUser(User user)
        {
            if (_users.Exists(x => x.UserId == user.UserId))
            {
                return true;
            }
            return false;
        }
        private bool ExistUser(int id)
        {
            if (_users.Exists(x => x.UserId == id))
            {
                return true;
            }
            return false;
        }
        public User FindUser(int id)
        {
            User user = _users.Find(x => x.UserId == id);
            return user;
        }
        public List<User> GetUsers()
        {
            return _users;
        }
        public void RemoveUser(int userId)
        {
            _users.RemoveAll(x => x.UserId == userId);
        }
        public void ChangeName(int userId, string newName)
        {
            User user = _users.Find(x => x.UserId == userId);
            if (user == null)
                throw new UserRepositoryException();
            user.UserName = newName;
        }
        public void AddReminder(int userId, Reminder reminder)
        {
            User user = _users.Find(x => x.UserId == userId);
            if (user == null)
                throw new UserRepositoryException();
            if (user.ReminderList.Count == 0)
            {
                reminder.Id = 0;
            }
            else
            {
                reminder.Id = user.ReminderList.Max(x => x.Id) + 1;
            }
            user.ReminderList.Add(reminder);
        }
        public void ChangeReminder(int userId, int id, Reminder reminder)
        {
            User user = _users.Find(x => x.UserId == userId);
            if (user == null)
                throw new UserRepositoryException();
            int index = user.ReminderList.FindIndex(x => x.Id == id);
            if (index < 0)
                throw new UserRepositoryException();
            user.ReminderList[index] = reminder;
        }
        public void RemoveReminder(int userId, int id)
        {
            User user = _users.Find(x => x.UserId == userId);
            if (user == null)
                throw new UserRepositoryException();
            user.ReminderList.RemoveAll(x => x.Id == id);
        }
    }
}
