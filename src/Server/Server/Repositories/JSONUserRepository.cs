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
        private List<User> Users = new();
        private readonly string _storageFileName;

        public JSONUserRepository() { }

        public JSONUserRepository(IConfiguration configuration)
        {
            _storageFileName = configuration.GetSection("Files").Get<FileConfiguration>().FileOfUsers;
        }

        public void AddUser(User user)
        {
            if (Users is null)
            {
                throw new NotFoundException();
            }
            if(Users.Exists(usr => usr.Equals(user)))
            {
                throw new AlreadyExistException();
            }
            user.Id = Users.Count == 0 ? 1 : Users.Max(evnt => evnt.Id) + 1;
            Users.Add(user);
        }

        public void DeleteUser(int id)
        {
            if (!IsExist(id))
            {
                throw new NotFoundException();
            }
            Users.Remove(Users.Single(user => user.Id == id));
        }

        public void DeleteAllUsers()
        {
            if (Users is null)
            {
                throw new NotFoundException();
            }
            Users.Clear();
        }

        public IEnumerable<User> GetUsers()
        {
            if (Users is null)
            {
                throw new NotFoundException();
            }
            return Users;
        }

        public User GetUser(int id)
        {
            if (!IsExist(id))
            {
                throw new NotFoundException();
            }
            return Users.Single(user => user.Id == id);
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
            if (!IsExist(id))
            {
                throw new NotFoundException();
            }
            if (Users.Where(usr => usr.Equals(user)).Count() > 1)
            {
                throw new AlreadyExistException();
            }
            var userFromRepo = Users.Single(us => us.Id == id);
            userFromRepo.Name = user.Name;
            userFromRepo.ChatId = user.ChatId;
            userFromRepo.Toggle = user.Toggle;
        }

        private bool IsExist(int id)
        {
            if (!Users.Exists(user => user.Id == id) || Users is null)
            {
                return false;
            }
            return true;
        }
    }
}
