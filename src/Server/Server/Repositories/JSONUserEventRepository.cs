using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Server.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Server.Repositories
{
    public class JSONUserEventRepository : IJSONUserEventRepository
    {
        public List<UserEvent> UserEvents { get; set; } = new();
        private readonly string _storageFileName;

        public JSONUserEventRepository(IConfiguration configuration)
        {
            _storageFileName = configuration.GetSection("Files").Get<FileConfiguration>().FileOfEvents;
        }
        public void AddUserEvent(UserEvent userEvent)
        {
            UserEvents.Add(userEvent);
            userEvent.Id = UserEvents.Max(evnt => evnt.Id) + 1;
        }

        public void DeleteUserEvent(int id)
        {
            UserEvents.Remove(UserEvents.Single(userEvent => userEvent.Id == id));
        }

        public void DeleteAllUserEvents()
        {
            UserEvents.Clear();
        }
        public IEnumerable<UserEvent> GetUserEvents()
        {
            return UserEvents;
        }
        public UserEvent GetUserEvent(int id)
        {
            return UserEvents.Exists(userEvent => userEvent.Id == id) ? UserEvents.Single(userEvent => userEvent.Id == id) : null;

        }

        public void LoadData()
        {
            if (!File.Exists(_storageFileName) || new FileInfo(_storageFileName).Length == 0)
            {
                UserEvents = new List<UserEvent>();
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

        public void UpdateUserEvent(int id, UserEvent uEvent)
        {
            if (UserEvents.Exists(uEvnt => uEvent.Id == id))
            {
                var userEvent = UserEvents.Single(us => us.Id == id);
                userEvent.User = uEvent.User;
                userEvent.EventName = uEvent.EventName;
                userEvent.DateNTime = uEvent.DateNTime;
                userEvent.EventFrequency = uEvent.EventFrequency;
                SaveData();
            }
        }
    }
}
