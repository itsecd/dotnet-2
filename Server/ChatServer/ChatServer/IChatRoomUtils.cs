using Grpc.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatServer
{
    public interface IChatRoomUtils
    {
        Task BroadcastMessage(Message message);
        void Join(string name, IServerStreamWriter<Message> responce);
        void Remove(string name);
        Task<KeyValuePair<string, IServerStreamWriter<Message>>?> SendMessageToSubsriber(KeyValuePair<string, IServerStreamWriter<Message>> user, Message message);
    }
}