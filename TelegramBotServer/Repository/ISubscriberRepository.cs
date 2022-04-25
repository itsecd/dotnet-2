using System.Collections.Generic;
using TelegramBotServer.Model;

namespace TelegramBotServer.Repository
{
    public interface ISubscriberRepository
    {
        public long AddSubscriber(Subscriber newSub);
        public void RemoveSubscriber(Subscriber sub);
        public void ChangeSubscriber(long id, Subscriber newSub);
        public Subscriber GetSubscriber(long id);
        IEnumerable<Subscriber> GetSubscribers();
    }
}
