using Grpc.Core;
using Snake;
using System.Threading.Tasks;

namespace Server.Services
{
    public class SnakeService : Snake.Snake.SnakeBase
    {

        private readonly GamingServer _server;

        public SnakeService(GamingServer server)
        {
            _server = server;
        }

        public override async Task Play(IAsyncStreamReader<Request> requestStream, IServerStreamWriter<Reply> responseStream, ServerCallContext context)
        {
            await _server.Play(requestStream, responseStream);
        }
    }
}
