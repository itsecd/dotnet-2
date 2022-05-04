using Microsoft.Extensions.Configuration;
using Server.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Server.Repositories
{
    public class JSONUserRepository : IJSONRepository<User>
    {
        public List<User> entyties { get; set; } = new();
        private string _storageFileName;
        private object _locker = new();

        public JSONUserRepository(IConfiguration configuration)
        {
            _storageFileName = configuration.GetSection("FileConfiguration").Get<FileConfiguration>().FileOfUsers;
        }
        public void Add(User user)
        {
            lock (_locker)
            {
                Load();
                entyties.Add(user);
                user.Id = entyties.Max(us => us.Id) + 1;
                Save();
            }
        }

        public void Delete(int id)
        {
            lock (_locker)
            {
                Load();
                entyties.Remove(entyties.Single(user => user.Id == id));
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
        public IEnumerable<User> Get()
        {
            lock (_locker)
            {
                Load();
                return entyties;
            }
        }
        public User Get(int id)
        {
            lock (_locker)
            {
                Load();
                return entyties.Exists(user => user.Id == id) ? entyties.Single(user => user.Id == id) : null;
            }
        }

        public void Load()
        {
            if (!File.Exists(_storageFileName) || new FileInfo(_storageFileName).Length == 0)
            {
                entyties = new List<User>();
                return;
            }
            using (var reader = new StreamReader(_storageFileName))
            {
                string jsonString = reader.ReadToEnd();
                entyties = JsonSerializer.Deserialize<List<User>>(jsonString);
            }
        }

        public void Save()
        {
            string jsonString = JsonSerializer.Serialize(entyties);
            using (var writer = new StreamWriter(_storageFileName))
            {
                writer.Write(jsonString);
            }
        }

        public void Update(int id, User user)
        {
            lock (_locker)
            {
                Load();
                if (entyties.Exists(user => user.Id == id))
                {
                    entyties.Single(us => us.Id == id).Name = user.Name;
                    entyties.Single(us => us.Id == id).Toggle = user.Toggle;
                    Save();
                }
            }
        }
    }
}
