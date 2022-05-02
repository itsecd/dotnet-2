using ChatServer.Networks;
using ChatServer.Repositories;
using Grpc.Core;
using System;
using System.Threading;
using System.Threading.Tasks;
namespace ChatServer.Services
{
    public class ChatService : ChatRoom.ChatRoomBase
    {

        private readonly IRoomRepository _chatRooms;
        private readonly IUserRepository _users;
        private static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        private static object _lock = new object();


        public ChatService(IRoomRepository chatRooms, IUserRepository users)
        {
            _chatRooms = chatRooms;
            _users = users;
        }

        public override async Task Create(IAsyncStreamReader<Message> requestStream, IServerStreamWriter<Message> responseStream, ServerCallContext context)
        {
            if (!await requestStream.MoveNext()) return;
            var userName = requestStream.Current.User;
            if (requestStream.Current.Command == "create")
            {
                if (_chatRooms.IsRoomExists(requestStream.Current.Text))
                {
                    await responseStream.WriteAsync(new Message { Text = "Данная комната существует!" });
                    return;
                }
                else
                {
                    var nameRoom = _chatRooms.AddRoom(requestStream.Current.Text, new RoomNetwork());
                    var room = _chatRooms.FindRoom(nameRoom);
                    _users.AddUser(userName);
                    room.Join(userName, responseStream);
                    room.AddUser(userName);
                    await _chatRooms.WriteAsyncToFile();
                    await _users.WriteAsyncToFile();
                    await responseStream.WriteAsync(new Message { Text = requestStream.Current.Text });
                    await requestStream.MoveNext();
                }
            }
            else
            {
                await responseStream.WriteAsync(new Message { Text = "Неверная команда!" });
                return;
            }

        }

        public override async Task Join(IAsyncStreamReader<Message> requestStream, IServerStreamWriter<Message> responseStream, ServerCallContext context)
        {
            var tcs = new TaskCompletionSource<int>();

            if (!await requestStream.MoveNext()) return;
            var nameRoom = requestStream.Current.Text;
            var userName = requestStream.Current.User;
            if (_chatRooms.IsRoomExists(nameRoom))
            {
                await semaphoreSlim.WaitAsync();
                try
                {
                    await _users.ReadFromFileAsync();
                    await _chatRooms.ReadFromFileAsync(nameRoom);
                }
                finally
                {
                    semaphoreSlim.Release();
                }
                var room = _chatRooms.FindRoom(nameRoom);

                room.Join(userName, responseStream);

                if (room.FindUser(userName))
                    room.AddUser(userName);

                if (_users.IsUserExist(userName))
                    _users.AddUser(userName);
                await semaphoreSlim.WaitAsync();
                try
                {
                    await _users.WriteAsyncToFile();
                }
                finally
                {
                    semaphoreSlim.Release();
                }

                await responseStream.WriteAsync(new Message { Text = "Connection success" });
                await room.BroadcastMessage(new Message { Text = $"{userName} connected" });
            }
            else
            {
                await responseStream.WriteAsync(new Message { Text = "No connection!" });
                return;
            }
            while (await requestStream.MoveNext())
            {
                switch (requestStream.Current.Command)
                {
                    case "message":
                        var CurrentMessage = requestStream.Current;
                        var room = _chatRooms.FindRoom(nameRoom);
                        await room.BroadcastMessage(CurrentMessage, userName);
                        break;
                    case "disconnect":
                        _chatRooms.FindRoom(nameRoom).Disconnect(requestStream.Current.User);
                        await responseStream.WriteAsync(new Message { Text = $"Пользователь {requestStream.Current.User} отключился!" });
                        break;
                    default:
                        Console.WriteLine(requestStream.Current);
                        break;
                }
                await semaphoreSlim.WaitAsync();
                try
                {
                    await _chatRooms.WriteAsyncToFile();
                }
                finally
                {
                    semaphoreSlim.Release();
                }

            }


        }
    }
}
