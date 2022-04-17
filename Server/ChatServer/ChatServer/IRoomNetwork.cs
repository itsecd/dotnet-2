using Grpc.Core;
using System.Threading.Tasks;

namespace ChatServer
{
    public interface IRoomNetwork
    {
        Task BroadcastMessage(Message message, string name = null);
        void Join(string name, IServerStreamWriter<Message> responce);
        void Remove(string name);
    }
}