using Grpc.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServer
{
    public class RoomNetwork : IRoomNetwork
    {
        public ConcurrentDictionary<string, IServerStreamWriter<Message>> Online { get; set; } = new();
        public ConcurrentBag<User> Users { get; set; } = new();
        public ConcurrentDictionary<DateTime, Message> History { get; set; } = new(); 

        //добавление
        public void Join(string name, IServerStreamWriter<Message> responce)
        {
            Online.TryAdd(name, responce);
            if (Users.Where(x => x.Name == name).Count() != 0)
                Users.Add(new User(name, name.GetHashCode() - 100000));


        }


        //удаление 
        public void Remove(string name)
        {
            Online.TryRemove(name, out _);
        }


        public async Task BroadcastMessage(Message message, string name = null)
        {
            History.TryAdd(DateTime.Now, message);
            foreach (var user in Users.Where(x => x.Name != name))
            {
                await Online[name].WriteAsync(message);
            }
        }
    }
}
