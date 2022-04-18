using ChatServer.Repositories;
using Grpc.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
namespace ChatServer.Services
{
    public class ChatService : ChatRoom.ChatRoomBase
    {

        private readonly IRoomRepository _chatRooms;
        private readonly IUserRepository _users;
        

        public ChatService(IRoomRepository chatRooms, IUserRepository users)
        {
            _chatRooms = chatRooms;
            _users = users;
        }

        public override async Task Create(IAsyncStreamReader<Message> requestStream, IServerStreamWriter<Message> responseStream, ServerCallContext context)
        {
            if (!await requestStream.MoveNext()) return;

            if (_chatRooms.IsRoomExists(requestStream.Current.Text))
            {
                await responseStream.WriteAsync(new Message { Text = "Данная комната существует!" });
                return;
            }
            else 
            {
                var nameRoom = _chatRooms.AddRoom(requestStream.Current.Text, new RoomNetwork());
                _users.AddUser(requestStream.Current.User);
                _chatRooms.FindRoom(requestStream.Current.Text).Online.TryAdd(requestStream.Current.User, responseStream);
                _chatRooms.FindRoom(requestStream.Current.Text).AddUser(requestStream.Current.User);
                _chatRooms.WriteToFile();
                await _users.WriteAsync();
                await responseStream.WriteAsync(new Message { Text = requestStream.Current.Text });
                await requestStream.MoveNext();
            }
            


            
        }

        public override async Task Join(IAsyncStreamReader<Message> requestStream, IServerStreamWriter<Message> responseStream, ServerCallContext context)
        {
            if (!await requestStream.MoveNext()) return;

            if (_chatRooms.IsRoomExists(requestStream.Current.Text))
            {
                await _chatRooms.ReadAsync(requestStream.Current.Text);
                _chatRooms.FindRoom(requestStream.Current.Text).Online.TryAdd(requestStream.Current.User, responseStream);
                if (_chatRooms.FindRoom(requestStream.Current.Text).Users.Where(x => x.Name == requestStream.Current.User).Count() == 0)
                    _chatRooms.FindRoom(requestStream.Current.Text).AddUser(requestStream.Current.User);
            }
            do
            {
                var CurrentMessage = requestStream.Current;
                var nameRoom = CurrentMessage.Text;
                _chatRooms.FindRoom(nameRoom).Join(requestStream.Current.User, responseStream);
                var room = _chatRooms.FindRoom(nameRoom);
                await room.BroadcastMessage(CurrentMessage);
                _chatRooms.FindRoom(nameRoom).History.TryAdd(DateTime.Now, new Message { User = requestStream.Current.User, Text = requestStream.Current.Text });
                   
            }
            while (await requestStream.MoveNext());
        }
    }
}
