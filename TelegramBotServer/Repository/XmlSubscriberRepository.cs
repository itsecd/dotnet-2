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

        public long AddSubscriber(Subscriber newSub)
        {
            ReadFile();
            _subs.Add(newSub);
            WriteFile();
            return newSub.Id;
        }

        public void ChangeSubscriber(long id, Subscriber newSub)
        {
            ReadFile();
            var index = _subs.IndexOf(_subs.Where(s => s.Id == id).FirstOrDefault());
            _subs[index] = newSub;
            WriteFile();
        }

        public Subscriber GetSubscriber(long id)
        {
            ReadFile();
            return _subs.Where(s => s.Id == id).FirstOrDefault();
        }

        public IEnumerable<Subscriber> GetSubscribers()
        {
            ReadFile();
            return _subs;
        }

        public void RemoveSubscriber(Subscriber sub)
        {
            ReadFile();
            _subs.Remove(sub);
            WriteFile();
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
