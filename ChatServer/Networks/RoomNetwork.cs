using Grpc.Core;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;


namespace ChatServer.Networks
{
    [Serializable]
    public class RoomNetwork : IRoomNetwork
    {
        public ConcurrentBag<User> Users { get; set; } = new();

        [NonSerialized] public ConcurrentDictionary<string, IServerStreamWriter<Message>> Online = new();

        public ConcurrentDictionary<DateTime, Message> History { get; set; } = new();

        public void AddUser(string name)
        {
            Users.Add(new User(name, Math.Abs(name.GetHashCode())));
        }

        public void Join(string name, IServerStreamWriter<Message> response) => Online.TryAdd(name, response);

        public void Disconnect(string name)
        {
            Online.TryRemove(name, out _);
        }


        public Task BroadcastMessage(Message message, string name = null)
        {
            History.TryAdd(DateTime.Now, message);
            return Task.WhenAll(Online.Where(x => x.Key != name).Select(userStream => userStream.Value.WriteAsync(message)));
        }

        public bool FindUser(string userName) => Users.Count(x => x.Name == userName) == 0;
    }
}
