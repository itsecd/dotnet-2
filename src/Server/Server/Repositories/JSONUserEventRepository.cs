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
        private List<UserEvent> _userEvents = new();
        private readonly string _storageFileName;

        public JSONUserEventRepository() { }

        public JSONUserEventRepository(IConfiguration configuration)
        {
            _storageFileName = configuration.GetSection("Files").Get<FileConfiguration>().FileOfEvents;
        }

        public void AddUserEvent(UserEvent userEvent)
        {
            if(_userEvents.Exists(usrEvnt => usrEvnt.Equals(userEvent)))
            {
                throw new AlreadyExistException();
            }
            var newUserEvent = new UserEvent()
            {
                Id = _userEvents.Count == 0 ? 1 : _userEvents.Max(evnt => evnt.Id) + 1,
                User = userEvent.User,
                EventName = userEvent.EventName,
                DateNTime = userEvent.DateNTime,
                EventFrequency = userEvent.EventFrequency,
            };
            _userEvents.Add(newUserEvent);
        }

        public void DeleteUserEvent(int id)
        {
            if (!IsExist(id))
            {
                throw new NotFoundException();
            }
            _userEvents.Remove(_userEvents.Single(userEvent => userEvent.Id == id));
        }

        public void DeleteAllUserEvents()
        {
            _userEvents.Clear();
        }

        public IEnumerable<UserEvent> GetUserEvents()
        {
            return _userEvents;
        }

        public UserEvent GetUserEvent(int id)
        {
            if (!IsExist(id))
            {
                throw new NotFoundException();
            }
            return _userEvents.Single(userEvent => userEvent.Id == id);
        }

        public void UpdateUserEvent(int id, UserEvent userEvent)
        {
            if (!IsExist(id))
            {
                throw new NotFoundException();
            }
            if (_userEvents.Count(usrEvnt => usrEvnt.Equals(userEvent)) > 1)
            {
                throw new AlreadyExistException();
            }
            var usrEvntFromRepo = _userEvents.Single(us => us.Id == id);
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
            _userEvents = JsonConvert.DeserializeObject<List<UserEvent>>(jsonString);
        }

        public void SaveData()
        {
            var jsonString = JsonConvert.SerializeObject(_userEvents);
            using var fileWriter = new StreamWriter(_storageFileName);
            fileWriter.Write(jsonString);
        }

        private bool IsExist(int id)
        {
            return _userEvents.Exists(user => user.Id == id);
        }
    }
}
