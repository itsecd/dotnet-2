using ChatServer.Repositories;
using Grpc.Core;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
namespace ChatServer.Services
{
    public class ChatService : ChatRoom.ChatRoomBase
    {

        private IChatRoomRepository _chatRooms; 
        

        public ChatService(IChatRoomRepository chatRooms)
        {
            
            _chatRooms = chatRooms;
            _chatRooms.ReadFile();

        }

     
        public override async Task Join(IAsyncStreamReader<Message> requestStream, IServerStreamWriter<Message> responseStream, ServerCallContext context)
        {
            int roomId = 0;
            if (!await requestStream.MoveNext()) return;
            if (requestStream.Current.Text == "Create")
            {
               roomId = _chatRooms.AddRoom(requestStream.Current.User.GetHashCode(), new ChatRoomUtils());
                _chatRooms.WriteToFile();
                await responseStream.WriteAsync(new Message{ Text = roomId.ToString()});
                await requestStream.MoveNext();
            }
            else
            {
                roomId = int.Parse(requestStream.Current.Text);
                var room = _chatRooms.FindRoom(roomId);
                await room.BroadcastMessage(requestStream.Current);
            }
            do
            {
                var CurrentMessage = requestStream.Current;
                var room = _chatRooms.FindRoom(roomId);
                await room.BroadcastMessage(CurrentMessage);
                   
            }
            while (await requestStream.MoveNext());
            var roomDelUser = _chatRooms.FindRoom(roomId);
            roomDelUser.Remove(context.Peer);
        }
    }
}
