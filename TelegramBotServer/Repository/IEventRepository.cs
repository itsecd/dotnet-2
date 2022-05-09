using System.Collections.Generic;
using TelegramBotServer.Model;

namespace TelegramBotServer.Repository
{
    public interface IEventRepository
    {
        public int AddEvent(Event newEvent);
        public bool RemoveEvent(int id);
        public bool ChangeEvent(int id, Event newEvent);
        public Event? GetEvent(int id);
        IEnumerable<Event>? GetEvents();
    }
}
