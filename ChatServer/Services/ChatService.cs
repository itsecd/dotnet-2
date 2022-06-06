using ChatServer.Networks;
using ChatServer.Repositories;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
namespace ChatServer.Services
{
    public class ChatService : ChatRoom.ChatRoomBase
    {

        private readonly IRoomRepository _chatRooms;
        private readonly IUserRepository _users;
        private readonly ILogger<ChatService> _logger;


        public ChatService(IRoomRepository chatRooms, IUserRepository users, ILogger<ChatService> logger)
        {
            _logger = logger;
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
                }
                else
                {
                    var nameRoom = _chatRooms.AddRoom(requestStream.Current.Text, new RoomNetwork());
                    var room = _chatRooms.FindRoom(nameRoom);
                    _users.AddUser(userName);
                    room.Join(userName, responseStream);
                    //перенос списка на общий доступ через proto
                    room.AddUser(userName);
                    await _chatRooms.WriteAsyncToFile();
                    await _users.WriteAsyncToFile();
                    await responseStream.WriteAsync(new Message { Text = $"Created new chat: {nameRoom}" });
                    //await requestStream.MoveNext();
                }
            }


        }

        public override async Task Join(IAsyncStreamReader<Message> requestStream, IServerStreamWriter<Message> responseStream, ServerCallContext context)
        {
            if (!await requestStream.MoveNext()) return;
            var nameRoom = requestStream.Current.Text;
            var userName = requestStream.Current.User;
            if (_chatRooms.IsRoomExists(nameRoom))
            {
                await _users.ReadFromFileAsync();
                await _chatRooms.ReadFromFileAsync(nameRoom);

                var room = _chatRooms.FindRoom(nameRoom);

                room.Join(userName, responseStream);

                if (room.FindUser(userName))
                    room.AddUser(userName);

                if (_users.IsUserExist(userName))
                    _users.AddUser(userName);
                await _users.WriteAsyncToFile();

                await _chatRooms.WriteAsyncToFile();
                await room.BroadcastMessage(new Message { Text = $"{userName} connected" });
                //дальше клиент не заходит
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
                        var currentMessage = requestStream.Current;
                        var room = _chatRooms.FindRoom(nameRoom);
                        await room.BroadcastMessage(currentMessage, userName);
                        break;
                    case "disconnect":
                        var currentRoom = _chatRooms.FindRoom(nameRoom);
                        currentRoom.Disconnect(requestStream.Current.User);
                        await currentRoom.BroadcastMessage(new Message { Text = $"{requestStream.Current.User} disconnected", Command = "disconnect" });
                        await _chatRooms.WriteAsyncToFile();
                        await _users.WriteAsyncToFile();
                        return;
                    default:
                        _logger.LogInformation(requestStream.Current.Text);
                        break;
                }

                await _chatRooms.WriteAsyncToFile();


            }


        }

        public async override Task<UsersInfoResponse> GetUsers(RoomInfo request, ServerCallContext context)
        {
            await _chatRooms.ReadFromFileAsync(request.RoomName);
            var room = _chatRooms.FindRoom(request.RoomName);
            var result = new UsersInfoResponse();

            foreach (var user in room.Users)
            {
                if (room.Online.ContainsKey(user.Name))
                {
                    result.Users.Add(new UserInfo { UserName = user.Name, Id = user.ID, IsOnline = true });
                }
                else
                    result.Users.Add(new UserInfo { UserName = user.Name, Id = user.ID, IsOnline = false });
            }

            return result;
        }

        public async override Task<HistoryOfMessages> GetHistoryOfMessages(RoomInfo request, ServerCallContext context)
        {
            await _chatRooms.ReadFromFileAsync(request.RoomName);
            var room = _chatRooms.FindRoom(request.RoomName);
            var result = new HistoryOfMessages();

            foreach (var message in room.History)
            {
                result.Messages.Add(new Message { User = message.Value.User, Text = message.Value.Text, Command = message.Value.Command });
                result.DateOfMessage.Add(message.Key.ToString());
            }

            return result;
        }
    }
}
