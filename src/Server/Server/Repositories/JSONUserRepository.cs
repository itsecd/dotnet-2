using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Server.Exceptions;
using Server.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Server.Repositories
{
    public class JSONUserRepository : IJSONUserRepository
    {
        private List<User> _users = new();
        private readonly string _storageFileName;

        public JSONUserRepository() { }

        public JSONUserRepository(IConfiguration configuration)
        {
            _storageFileName = configuration.GetSection("Files").Get<FileConfiguration>().FileOfUsers;
        }

        public void AddUser(User user)
        {
            if(_users.Exists(usr => usr.Equals(user)))
            {
                throw new AlreadyExistException();
            }
            var newUser = new User()
            {
                Id = _users.Count == 0 ? 1 : _users.Max(usr => usr.Id) + 1,
                Name = user.Name,
                ChatId = user.ChatId,
                Toggle = user.Toggle,
            };
            _users.Add(newUser);
        }

        public void DeleteUser(int id)
        {
            if (!IsExist(id))
            {
                throw new NotFoundException();
            }
            _users.Remove(_users.Single(user => user.Id == id));
        }

        public void DeleteAllUsers()
        {
            _users.Clear();
        }

        public IEnumerable<User> GetUsers()
        {
            return _users;
        }

        public User GetUser(int id)
        {
            if (!IsExist(id))
            {
                throw new NotFoundException();
            }
            return _users.Single(user => user.Id == id);
        }

        public void LoadData()
        {
            if (!File.Exists(_storageFileName) || new FileInfo(_storageFileName).Length == 0)
            {
                _users = new List<User>();
                return;
            }
            using var reader = new StreamReader(_storageFileName);
            var jsonString = reader.ReadToEnd();
            _users = JsonConvert.DeserializeObject<List<User>>(jsonString);
        }

        public void SaveData()
        {
            var jsonString = JsonConvert.SerializeObject(_users);
            using var writer = new StreamWriter(_storageFileName);
            writer.Write(jsonString);
        }

        public void UpdateUser(int id, User user)
        {
            if (!IsExist(id))
            {
                throw new NotFoundException();
            }
            if (_users.Count(usr => usr.Equals(user)) > 1)
            {
                throw new AlreadyExistException();
            }
            var userFromRepo = _users.Single(us => us.Id == id);
            userFromRepo.Name = user.Name;
            userFromRepo.ChatId = user.ChatId;
            userFromRepo.Toggle = user.Toggle;
        }

        private bool IsExist(int id)
        {
            return _users.Exists(user => user.Id == id);
        }
    }
}
