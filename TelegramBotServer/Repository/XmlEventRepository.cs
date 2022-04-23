using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TelegramBotServer.Model;
using System.Linq;

namespace TelegramBotServer.Repository
{
    public class XmlEventRepository : IEventRepository
    {
        private readonly string _filePath;
        private List<Event> _events;
        
        public XmlEventRepository(IConfiguration config)
        {
            _filePath = config["XmlDatabaseFile"];
        }

        public int AddEvent(Event newEvent)
        {
            ReadFile();
            _events.Add(newEvent);
            WriteFile();
            return newEvent.Id;
        }

        public void ChangeEvent(int id, Event newEvent)
        {
            ReadFile();
            var index  = _events.IndexOf(_events.Where(e => e.Id == id).FirstOrDefault());
            _events[index] = newEvent;
            WriteFile();
        }

        public Event GetEvent(int id)
        {
            ReadFile();
            return _events.Where(e => e.Id == id).FirstOrDefault();
        }

        public IEnumerable<Event> GetEvents()
        {
            ReadFile();
            return _events;
        }

        public void RemoveEvent(Event someEvent)
        {
            ReadFile();
            _events.Remove(someEvent);
            WriteFile();
        }

        private void ReadFile()
        {
            if (_events != null)
                return;

            if (!File.Exists(_filePath))
            {
                _events = new();
                return;
            }

            var xmlSerializer = new XmlSerializer(typeof(List<Event>));
            using var fileStream = new FileStream(_filePath, FileMode.Open);
            _events = (List<Event>)xmlSerializer.Deserialize(fileStream);
        }

        private void WriteFile()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Event>));
            using var fileStream = new FileStream(_filePath, FileMode.Create);
            xmlSerializer.Serialize(fileStream, _events);
        }

    }
}
