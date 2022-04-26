using System.Threading.Tasks;

namespace ChatServer.Repositories
{
    public interface IRoomRepository
    {
        string AddRoom(string nameRoom, RoomNetwork room);
        RoomNetwork FindRoom(string nameRoom);
        Task ReadAsync(string nameRoom);
        void RemoveRoom(string nameRoom);
        Task WriteAsync();

        public bool IsRoomExists(string nameRoom);
    }
}