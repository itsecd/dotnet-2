using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using TelegramBotServer.Model;

namespace TelegramBotServer.Repository
{
    public class XmlSubscriberRepository : ISubscriberRepository
    {
        private readonly string _filePath;
        private List<Subscriber> _subs;
        public XmlSubscriberRepository(IConfiguration config)
        {
            _filePath = config["XmlSubscriberDatabaseFile"];
        }

        public int AddSubscriber(Subscriber newSub)
        {
            ReadFile();
            _subs.Add(newSub);
            WriteFile();
            return newSub.Id;
        }

        public bool ChangeSubscriber(int id, Subscriber newSub)
        {
            ReadFile();
            var index = _subs.IndexOf(_subs.FirstOrDefault(s => s.Id == id));
            _subs[index] = newSub;
            WriteFile();
            return true;
        }

        public Subscriber GetSubscriber(int id)
        {
            ReadFile();
            return _subs.Where(s => s.Id == id).FirstOrDefault();
        }

        public IEnumerable<Subscriber> GetSubscribers()
        {
            ReadFile();
            return _subs;
        }

        public bool RemoveSubscriber(int id)
        {
            ReadFile();
            var delSub = _subs.FirstOrDefault(s => s.Id == id);
            if (delSub is null)
                return false;
            _subs.Remove(delSub);
            WriteFile();
            return true;
        }

        private void ReadFile()
        {
            if (_subs != null)
                return;

            if (!File.Exists(_filePath))
            {
                _subs = new();
                return;
            }

            var xmlSerializer = new XmlSerializer(typeof(List<Subscriber>));
            using var fileStream = new FileStream(_filePath, FileMode.Open);
            _subs = (List<Subscriber>)xmlSerializer.Deserialize(fileStream);
        }

        private void WriteFile()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Subscriber>));
            using var fileStream = new FileStream(_filePath, FileMode.Create);
            xmlSerializer.Serialize(fileStream, _subs);
        }
    }
}
