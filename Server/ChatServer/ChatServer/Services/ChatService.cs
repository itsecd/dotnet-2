using Grpc.Core;
using System.Threading.Tasks;
namespace ChatServer.Services
{
    public class ChatService : ChatRoom.ChatRoomBase
    {
        private readonly IChatRoomUtils _chatRoomUtils;

        public ChatService(IChatRoomUtils chatRoomUtils)
        {
            _chatRoomUtils = chatRoomUtils;
        }
        public override async Task Join(IAsyncStreamReader<Message> requestStream, IServerStreamWriter<Message> responseStream, ServerCallContext context)
        {
            if (!await requestStream.MoveNext()) return; // если все закончилось - возвращаемся

            do
            {
                // регистрация пользоваетля
                _chatRoomUtils.Join(requestStream.Current.User, responseStream);
                await _chatRoomUtils.BroadcastMessage(requestStream.Current);
            }
            while (await requestStream.MoveNext()); // пока в потоке вопросов что-то имеется

            _chatRoomUtils.Remove(context.Peer);
        }
    }
}
