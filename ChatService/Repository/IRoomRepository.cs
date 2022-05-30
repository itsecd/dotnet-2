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
        Task BroadcastMessage(Message message, string roomName);
        Task ReadFileAsync(string name);
        Task WriteFileAsync();
        Room FindRoom(string roomName);
    }
}
