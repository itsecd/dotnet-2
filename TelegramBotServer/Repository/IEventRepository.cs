using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelegramBotServer.Model;

namespace TelegramBotServer.Repository
{
    public interface IEventRepository
    {
        public int AddEvent(Event newEvent);
        public void RemoveEvent(Event someEvent);
        public void ChangeEvent(int id, Event newEvent);
        public Event GetEvent(int id);
        IEnumerable<Event> GetEvents();
    }
}
