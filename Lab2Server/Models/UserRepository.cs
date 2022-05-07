using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Extensions.Configuration;

namespace Lab2Server.Models
{
    public class UserRepository : IUserRepository
    {
        public UserRepository(IConfiguration configuration)
        {
            _storageFileName = configuration.GetValue<string>("RepositoryFilePath");
        }
        
        private readonly string _storageFileName = "users.xml";

        private List<User> _users;
        private void ReadFromFile()
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

        private void WriteToFile()
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
            ReadFromFile();
            _users.Add(newUser);
            WriteToFile();
        }
        public bool ExistUser(User user)
        {
            ReadFromFile();
            if (_users.Exists(x => x.UserId == user.UserId))
            {
                return true;
            }
            return false;
        }
        public User FindUser(int id)
        {
            ReadFromFile();
            User user = _users.Find(x => x.UserId == id);
            return user;
        }
        public void RemoveUser(int userId)
        {
            ReadFromFile();
            _users.RemoveAll(x => x.UserId == userId);
            WriteToFile();
        }
        public void ChangeName(int userId, string newName)
        {
            ReadFromFile();
            User user = _users.Find(x => x.UserId == userId);
            if (user == null)
                throw new UserRepositoryException();
            user.UserName = newName;
            WriteToFile();
        }
        public void AddReminder(int userId, Reminder reminder)
        {
            ReadFromFile();
            User user = _users.Find(x => x.UserId == userId);
            if (user == null)
                throw new UserRepositoryException();
            reminder.Id = user.ReminderList.Max(x => x.Id) + 1;
            user.ReminderList.Add(reminder);
            WriteToFile();
        }
        public void ChangeReminder(int userId, int id, Reminder reminder)
        {
            ReadFromFile();
            User user = _users.Find(x => x.UserId == userId);
            if (user == null)
                throw new UserRepositoryException();
            int index = user.ReminderList.FindIndex(x => x.Id == id);
            if (index < 0)
                throw new UserRepositoryException();
            user.ReminderList[index] = reminder;
            WriteToFile();
        }
        public void RemoveReminder(int userId, int id)
        {
            ReadFromFile();
            User user = _users.Find(x => x.UserId == userId);
            if (user == null)
                throw new UserRepositoryException();
            user.ReminderList.RemoveAll(x => x.Id == id);
            WriteToFile();
        }
    }
}
