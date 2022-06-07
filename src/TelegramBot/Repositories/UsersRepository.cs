using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using TelegramBot.Model;

namespace TelegramBot.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly string _storageFileName;

        private List<User> _users;

        public UsersRepository(IConfiguration configuration = null)
        {
            if (configuration != null)
            {
                _storageFileName = configuration.GetValue<string>("RepositoryFilePath");
            }
            else
            {
                _storageFileName = "Users.xml";
            }
            _users = new List<User>();
        }

        public List<User> GetUsers()
        {
            return _users;
        }

        public void ReadFromFile()
        {
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
;
            _users.Add(user);
        }

        public void RemoveUser(long userId)
        {
            _users.RemoveAll(x => x.UserId == userId);
        }

        public void AddEvent(long userId, UserEvent userEvent)
        {
            var user = FindUser(userId);
            if (user.Events.Count == 0)
            {
                userEvent.Id = 0;
            }
            else
            {
                userEvent.Id = user.Events.Max(x => x.Id) + 1;
            }
            user.Events.Add(userEvent);
        }

        public void ChangeEvent(long userId, long id, UserEvent userEvent)
        {
            var user = FindUser(userId);
            var eventId = user.Events.FindIndex(x => x.Id == id);
            user.Events[eventId] = userEvent;
        }

        public void RemoveEvent(long userId, long id)
        {
            var user = FindUser(userId);
            user.Events.RemoveAll(x => x.Id == id);
        }
    }
}
