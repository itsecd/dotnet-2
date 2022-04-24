using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelegramBotServer.Model;

namespace TelegramBotServer.Repository
{
    public interface ISubscriberRepository
    {
        public int AddSubscriber(Subscriber newSub);
        public void RemoveSubscriber(Subscriber sub);
        public void ChangeSubscriber(int id, Subscriber newSub);
        public Subscriber GetSubscriber(int id);
        IEnumerable<Subscriber> GetSubscribers();
    }
}
