using System.Collections.Generic;
using TelegramBotServer.Model;

namespace TelegramBotServer.Repository
{
    public interface ISubscriberRepository
    {
        public int AddSubscriber(Subscriber newSub);
        public bool RemoveSubscriber(int id);
        public bool ChangeSubscriber(int id, Subscriber newSub);
        public Subscriber? GetSubscriber(int id);
        IEnumerable<Subscriber>? GetSubscribers();
    }
}
