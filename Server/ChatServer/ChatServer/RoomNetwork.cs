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
        private ConcurrentBag<User> _users { get; set; } = new();
        [NonSerialized] private ConcurrentDictionary<string, IServerStreamWriter<Message>> _online  = new();
        //public ConcurrentBag<User> User 
        public ConcurrentDictionary<DateTime, Message> History { get; set; } = new();

        public void AddUser(string name) {
            _users.Add(new User(name, name.GetHashCode()));
        }
        public ConcurrentBag<User> GetUsers() => _users;
        public void Join(string name, IServerStreamWriter<Message> responce) => _online.TryAdd(name, responce);
       
        public void Disconnect(string name)
        {
            _online.TryRemove(name, out _);
        }


        public async Task BroadcastMessage(Message message, string name = null)
        {
            History.TryAdd(DateTime.Now, message);
            foreach (var user in GetUsers().Where(x => x.Name != name))
            {
                await _online[user.Name].WriteAsync(message);
            }
        }

        public bool FindUser(string userName) => _users.Where(x => x.Name == userName).Count() == 0;
    }
}
