using ChatService.Models;
using Grpc.Core;
using System.Threading.Tasks;

namespace ChatService.Repository
{
    public interface IRoomRepository
    {
        void AddRoom(string name);
        void Join(string roomName, string userName, IServerStreamWriter<Message> response);
        void Disconnect(string roomName, string userName);
        Task BroadcastMessage(Message message, string roomName, string userName);
        Task ReadFileAsync(string name);
        Task WriteFileAsync();
        public Room FindRoom(string roomName);
    }
}
