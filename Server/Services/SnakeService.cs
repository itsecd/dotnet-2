using Grpc.Core;
using System.Threading.Tasks;

namespace Server.Services
{
    public class SnakeService:Snake.SnakeBase
    {
        private readonly ChatRoomUtils _chatRoomUtils;

        public SnakeService(ChatRoomUtils chatRoomUtils)
        {
            _chatRoomUtils = chatRoomUtils;
        }

        public override async Task Join(IAsyncStreamReader<PlayerMessage> requestStream, IServerStreamWriter<PlayerMessage> responseStream, ServerCallContext context)
        {
            if(!await requestStream.MoveNext()) return;
            do
            {
                _chatRoomUtils.Join(requestStream.Current.Name, responseStream);
                await _chatRoomUtils.BroadCastMessage(requestStream.Current);
            }while(await requestStream.MoveNext());

            _chatRoomUtils.Remove(context.Peer);
        }
    }
}
