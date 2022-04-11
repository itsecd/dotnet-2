using Grpc.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServer
{
    public class ChatRoomUtils : IChatRoomUtils
    {
        public ConcurrentDictionary<string, User> Users = new();
        public ConcurrentDictionary<DateTime, MessageInfo> Messages = new();
        private readonly ConcurrentDictionary<string, IServerStreamWriter<Message>> _users = new();

        //добавление
        public void Join(string name, IServerStreamWriter<Message> responce) {
            _users.TryAdd(name, responce);
            Users.TryAdd(name, new User(name, name.GetHashCode() - 100000));
            
           
        }
            

        //удаление 
        public void Remove(string name) => _users.TryRemove(name, out var s);



        public async Task<KeyValuePair<string, IServerStreamWriter<Message>>?> SendMessageToSubsriber(
            KeyValuePair<string, IServerStreamWriter<Message>> user, Message message)
        {
            try
            {
                await user.Value.WriteAsync(message);
                return null;
            }
            catch (Exception)
            {
                return user;
            }

        }

        public async Task BroadcastMessage(Message message)
        {
            Messages.TryAdd(DateTime.Now, new MessageInfo { User = Users[message.User], Text = message.Text});
            foreach (var user in _users.Where(x => x.Key != message.User))
            {
                var result = await SendMessageToSubsriber(user, message);

                if (result != null)
                {
                    Remove(result.Value.Key);
                }
            }
        }
    }
}
