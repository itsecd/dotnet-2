using ChatService.Exceptions;
using ChatService.Repository;
using Grpc.Core;
using System.Threading.Tasks;

namespace ChatService.Services
{
    public class ChatServerService : Chat.ChatBase
    {
        private readonly IRoomRepository _repository;
        public ChatServerService(IRoomRepository repository)
        {
            _repository = repository;
        }

        public override async Task Create(IAsyncStreamReader<Message> requestStream, IServerStreamWriter<Message> responseStream, ServerCallContext context)
        {
            if (!await requestStream.MoveNext())
                return;
            try
            {
                _repository.AddRoom(requestStream.Current.RoomName);
                await _repository.WriteFileAsync();
                await responseStream.WriteAsync(new Message { Text = requestStream.Current.Text });
                await requestStream.MoveNext();
            }
            catch (NameIsUseException)
            {
                await responseStream.WriteAsync(new Message { Text = "Room has been!" });
            }
        }

        public override async Task Join(IAsyncStreamReader<Message> requestStream, IServerStreamWriter<Message> responseStream, ServerCallContext context)
        {
            try
            {
                if (!await requestStream.MoveNext())
                    return;
                if (_repository.FindRoom(requestStream.Current.RoomName) != null)
                {
                    await _repository.ReadFileAsync(requestStream.Current.RoomName);

                    _repository.Join(requestStream.Current.RoomName, requestStream.Current.UserName, responseStream);
                    await responseStream.WriteAsync(new Message { Text = "Connection success" });
                    await _repository.BroadcastMessage(
                        new Message { Text = $"{requestStream.Current.UserName} connected", UserName = requestStream.Current.UserName },
                        requestStream.Current.RoomName
                        );
                }
                else
                {
                    await responseStream.WriteAsync(new Message { Text = "No connection!" });
                    return;
                }
                while (await requestStream.MoveNext())
                {
                    await _repository.BroadcastMessage(requestStream.Current, requestStream.Current.RoomName);
                    await _repository.WriteFileAsync();
                }
                await _repository.WriteFileAsync();
                _repository.Disconnect(requestStream.Current.RoomName, requestStream.Current.UserName);
            }
            catch (NameIsUseException)
            {
                await responseStream.WriteAsync(new Message { Text = "User has been!" });
            }
        }
    }
}
