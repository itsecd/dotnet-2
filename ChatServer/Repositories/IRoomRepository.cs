using ChatServer.Networks;
using System.Threading.Tasks;

namespace ChatServer.Repositories
{
    public interface IRoomRepository
    {
        string AddRoom(string nameRoom, RoomNetwork room);
        RoomNetwork FindRoom(string nameRoom);
        Task ReadFromFileAsync(string nameRoom);
        void RemoveRoom(string nameRoom);
        Task WriteAsyncToFile();

        bool IsRoomExists(string nameRoom);
    }
}