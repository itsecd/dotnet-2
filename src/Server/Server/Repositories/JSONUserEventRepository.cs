using Microsoft.Extensions.Configuration;
using Server.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Server.Repositories
{
    public class JSONUserEventRepository : IJSONUserEventRepository
    {
        public List<UserEvent> entyties { get; set; } = new();
        private readonly string _storageFileName;

        public JSONUserEventRepository(IConfiguration configuration)
        {
            _storageFileName = configuration.GetSection("Files").Get<FileConfiguration>().FileOfEvents;
        }
        public void AddUserEvent(UserEvent uEvent)
        {
            entyties.Add(uEvent);
            uEvent.Id = entyties.Max(evnt => evnt.Id) + 1;
        }

        public void DeleteUserEvent(int id)
        {
            entyties.Remove(entyties.Single(uEvent => uEvent.Id == id));
        }

        public void DeleteAllUserEvents()
        {
            entyties.Clear();
        }
        public IEnumerable<UserEvent> GetUserEvents()
        {
            return entyties;
        }
        public UserEvent GetUserEvent(int id)
        {
            return entyties.Exists(userEvent => userEvent.Id == id) ? entyties.Single(userEvent => userEvent.Id == id) : null;
        }

        public void LoadData()
        {
            if (!File.Exists(_storageFileName) || new FileInfo(_storageFileName).Length == 0)
            {
                entyties = new List<UserEvent>();
                return;
            }
            using var fileReader = new StreamReader(_storageFileName);
            string jsonString = fileReader.ReadToEnd();
            entyties = JsonSerializer.Deserialize<List<UserEvent>>(jsonString);
        }

        public void SaveData()
        {
            string jsonString = JsonSerializer.Serialize(entyties);
            using var fileWriter = new StreamWriter(_storageFileName);
            fileWriter.Write(jsonString);
        }

        public void UpdateUserEvent(int id, UserEvent uEvent)
        {
            if (entyties.Exists(uEvnt => uEvent.Id == id))
            {
                UserEvent userEvent = entyties.Single(us => us.Id == id);
                userEvent.User = uEvent.User;
                userEvent.EventName = uEvent.EventName;
                userEvent.DateNTime = uEvent.DateNTime;
                userEvent.EventFrequency = uEvent.EventFrequency;
                SaveData();
            }
        }
    }
}
