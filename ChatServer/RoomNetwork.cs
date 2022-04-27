using Grpc.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServer
{
    [Serializable]
    public class RoomNetwork : IRoomNetwork
    {
        public ConcurrentBag<User> Users { get; set; } = new();

        [NonSerialized] public ConcurrentDictionary<string, IServerStreamWriter<Message>> Online  = new();
       
        public ConcurrentDictionary<DateTime, Message> History { get; set; } = new();

        public void AddUser(string name) {
            Users.Add(new User(name, name.GetHashCode()));
        }
        
        public void Join(string name, IServerStreamWriter<Message> responce) => Online.TryAdd(name, responce);
       
        public void Disconnect(string name)
        {
            Online.TryRemove(name, out _);
        }


        public async Task BroadcastMessage(Message message, string name = null)
        {
            History.TryAdd(DateTime.Now, message);
            foreach (var (username, channel) in Online.Where(x => x.Key != name))
            {
                await channel.WriteAsync(message);
            }
        }

        public bool FindUser(string userName) => Users.Where(x => x.Name == userName).Count() == 0;
    }
}
