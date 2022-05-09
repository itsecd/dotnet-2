using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Server.Exceptions;
using Server.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Server.Repositories
{
    public class JSONUserEventRepository : IJSONUserEventRepository
    {
        private List<UserEvent> UserEvents = new();
        private readonly string _storageFileName;

        public JSONUserEventRepository() { }

        public JSONUserEventRepository(IConfiguration configuration)
        {
            _storageFileName = configuration.GetSection("Files").Get<FileConfiguration>().FileOfEvents;
        }

        public void AddUserEvent(UserEvent userEvent)
        {
            if (UserEvents is null)
            {
                throw new NotFoundException();
            }
            if(UserEvents.Exists(usrEvnt => usrEvnt.Equals(userEvent)))
            {
                throw new AlreadyExistException();
            }
            userEvent.Id = UserEvents.Count == 0 ? 1 : UserEvents.Max(evnt => evnt.Id) + 1;
            UserEvents.Add(userEvent);
        }

        public void DeleteUserEvent(int id)
        {
            if (!IsExist(id))
            {
                throw new NotFoundException();
            }
            UserEvents.Remove(UserEvents.Single(userEvent => userEvent.Id == id));
        }

        public void DeleteAllUserEvents()
        {
            if (UserEvents is null)
            {
                throw new NotFoundException();
            }
            UserEvents.Clear();
        }

        public IEnumerable<UserEvent> GetUserEvents()
        {
            if (UserEvents is null)
            {
                throw new NotFoundException();
            }
            return UserEvents;
        }

        public UserEvent GetUserEvent(int id)
        {
            if (!IsExist(id))
            {
                throw new NotFoundException();
            }
            return UserEvents.Single(userEvent => userEvent.Id == id);
        }

        public void UpdateUserEvent(int id, UserEvent userEvent)
        {
            if (!IsExist(id))
            {
                throw new NotFoundException();
            }
            if (UserEvents.Where(usrEvnt => usrEvnt.Equals(userEvent)).Count() > 1)
            {
                throw new AlreadyExistException();
            }
            var usrEvntFromRepo = UserEvents.Single(us => us.Id == id);
            usrEvntFromRepo.EventName = userEvent.EventName;
            usrEvntFromRepo.User = new User()
            {
                Id = userEvent.User.Id,
                Name = userEvent.User.Name,
                ChatId = userEvent.User.ChatId,
                Toggle = userEvent.User.Toggle,
            };
            usrEvntFromRepo.DateNTime = userEvent.DateNTime;
            usrEvntFromRepo.EventFrequency = userEvent.EventFrequency;

        }

        public void LoadData()
        {
            if (!File.Exists(_storageFileName) || new FileInfo(_storageFileName).Length == 0)
            { 
                return;
            }
            using var fileReader = new StreamReader(_storageFileName);
            var jsonString = fileReader.ReadToEnd();
            UserEvents = JsonConvert.DeserializeObject<List<UserEvent>>(jsonString);
        }

        public void SaveData()
        {
            var jsonString = JsonConvert.SerializeObject(UserEvents);
            using var fileWriter = new StreamWriter(_storageFileName);
            fileWriter.Write(jsonString);
        }

        private bool IsExist(int id)
        {
            if (!UserEvents.Exists(user => user.Id == id) || UserEvents is null)
            {
                return false;
            }
            return true;
        }
    }
}
