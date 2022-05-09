using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using TelegramBotServer.Model;

namespace TelegramBotServer.Repository
{
    public class XmlEventRepository : IEventRepository
    {
        private readonly string _filePath;
        private List<Event>? _events;

        public XmlEventRepository(IConfiguration config)
        {
            _filePath = config["XmlEventDatabaseFile"];
        }

        public int AddEvent(Event newEvent)
        {
            ReadFile();
            _events?.Add(newEvent);
            WriteFile();
            return newEvent.Id;
        }

        public void ChangeEvent(int id, Event newEvent)
        {
            ReadFile();
            if (_events is null)
                return;
            else {
                var someEvent = _events.FirstOrDefault(e => e.Id == id);
                if (someEvent is null)
                    return;
                int index = _events.IndexOf(someEvent);
                _events[index] = newEvent;
                WriteFile();
            }
        }

        public Event? GetEvent(int id)
        {
            ReadFile();
            return _events?.FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<Event>? GetEvents()
        {
            ReadFile();
            return _events;
        }

        public bool RemoveEvent(int id)
        {
            ReadFile();
            var delEvent = _events?.Find(e => e.Id == id);
            if (delEvent is null) 
                return false;
            _events?.Remove(delEvent);
            WriteFile();
            return true;
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
            _events = (List<Event>?)xmlSerializer.Deserialize(fileStream);
        }

        private void WriteFile()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Event>));
            using var fileStream = new FileStream(_filePath, FileMode.Create);
            xmlSerializer.Serialize(fileStream, _events);
        }

    }
}
