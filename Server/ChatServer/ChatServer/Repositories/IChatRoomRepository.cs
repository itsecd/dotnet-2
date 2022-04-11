using System.Threading.Tasks;

namespace ChatServer.Repositories
{
    public interface IChatRoomRepository
    {
        int AddRoom(int id, ChatRoomUtils room);
        Task ReadAsync();
        void ReadFile();
        void RemoveRoom(int id);
        Task WriteAsync();
        void WriteToFile();
        ChatRoomUtils FindRoom(int id);
    }
}