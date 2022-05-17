using System.Collections.Generic;
using TelegramBotServer.Model;

namespace TelegramBotServer
{
    public class SubscriberSessions
    {
        public Dictionary<long, SubscriberSession> Sessions { get; set; }
        public SubscriberSessions()
        {
            Sessions = new Dictionary<long, SubscriberSession>();
        }
        public SubscriberSession this[long index]
        {
            get => Sessions[index];
            set => Sessions[index] = value;
        }
    }
}
