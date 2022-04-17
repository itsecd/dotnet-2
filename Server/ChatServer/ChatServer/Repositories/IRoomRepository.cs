using System.Threading.Tasks;

namespace ChatServer.Repositories
{
    public interface IRoomRepository
    {
        string AddRoom(string nameRoom, RoomNetwork room);
        RoomNetwork FindRoom(string nameRoom);
        Task ReadAsync(string nameRoom);
        void ReadFile(string nameRoom);
        void RemoveRoom(string nameRoom);
        Task WriteAsync();
        void WriteToFile();
        public bool IsRoomExists(string nameRoom);
    }
}