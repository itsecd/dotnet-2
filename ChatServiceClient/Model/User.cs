using Grpc.Core;

namespace ChatService.Model
{
    public class User
    {
        public string UserName { get; set; }
        public string RoomName { get; set; }
        
        public User(string userName, string roomName)
        {
            UserName = userName;
            RoomName = roomName;
        }

    }
}
