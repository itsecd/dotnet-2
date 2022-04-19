using Grpc.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public class ChatRoomUtils
    {
        private readonly ConcurrentDictionary<string, IServerStreamWriter<PlayerMessage>> _users = new ();
        public void Join(string name, IServerStreamWriter<PlayerMessage> responce) => _users.TryAdd(name, responce);

        public void Remove(string name) => _users.TryRemove(name, out var s);

        private async Task<KeyValuePair<string, IServerStreamWriter<PlayerMessage>>?> SendMessageToSubsriver(
           KeyValuePair<string, IServerStreamWriter<PlayerMessage>> user, PlayerMessage message)
        {
            try
            {
                await user.Value.WriteAsync(message);
                return null;
            }
            catch(Exception)
            {
                return user;
            }
        }

        public async Task BroadCastMessage(PlayerMessage message)
        {
            foreach (var user in _users.Where(x => x.Key != message.Name))
            {
                var result = await SendMessageToSubsriver(user,message);
                if (result != null)
                {
                    Remove(result.Value.Key);
                }
            }
        }
    }
}
