using Grpc.Core;
using System.Threading.Tasks;

namespace ChatServer.Networks
{
    public interface IRoomNetwork
    {
        Task BroadcastMessage(Message message, string name = null);
        void Join(string name, IServerStreamWriter<Message> response);
        void Disconnect(string name);

        public bool FindUser(string userName);
    }
}