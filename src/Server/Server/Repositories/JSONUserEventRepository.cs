using Microsoft.Extensions.Configuration;
using Server.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Server.Repositories
{
    public class JSONUserEventRepository : IJSONRepository<UserEvent>
    {
        public List<UserEvent> entyties { get; set; } = new();
        private string _storageFileName;
        private object _locker = new();
        
        public JSONUserEventRepository(IConfiguration configuration)
        {
            _storageFileName = configuration.GetSection("Files").Get<FileConfiguration>().FileOfEvents;
        }
        public void Add(UserEvent uEvent)
        {
            lock (_locker)
            {
                Load();
                entyties.Add(uEvent);
                uEvent.Id = entyties.Max(evnt => evnt.Id) + 1;
                Save();
            }
        }

        public void Delete(int id)
        {
            lock (_locker)
            {
                Load();
                entyties.Remove(entyties.Single(uEvent => uEvent.Id == id));
                Save();
            }
        }

        public void DeleteAll()
        {
            lock (_locker)
            {
                Load();
                entyties.Clear();
                Save();
            }
        }
        public IEnumerable<UserEvent> Get()
        {
            lock (_locker)
            {
                Load();
                return entyties;
            }
        }
        public UserEvent Get(int id)
        {
            lock (_locker)
            {
                Load();
                return entyties.Exists(userEvent => userEvent.Id == id) ? entyties.Single(userEvent => userEvent.Id == id) : null;
            }
        }

        public void Load()
        {
            if (!File.Exists(_storageFileName) || new FileInfo(_storageFileName).Length == 0)
            {
                entyties = new List<UserEvent>();
                return;
            }
            using (var fileReader = new StreamReader(_storageFileName))
            {
                string jsonString = fileReader.ReadToEnd();
                entyties = JsonSerializer.Deserialize<List<UserEvent>>(jsonString);
            }
        }

        public void Save()
        {
            string jsonString = JsonSerializer.Serialize(entyties);
            using (var fileWriter = new StreamWriter(_storageFileName))
            {
                fileWriter.Write(jsonString);
            }
        }

        public void Update(int id, UserEvent uEvent)
        {
            lock (_locker)
            {
                Load();
                if (entyties.Exists(uEvnt => uEvent.Id == id))
                {
                    UserEvent userEvent = entyties.Single(us => us.Id == id);
                    userEvent.User = uEvent.User;
                    userEvent.EventName = uEvent.EventName;
                    userEvent.DateNTime = uEvent.DateNTime;
                    userEvent.EventFrequency = uEvent.EventFrequency;
                    Save();
                }
            }
        }
    }
}
