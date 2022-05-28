using System.Threading.Tasks;

using Gomoku;

using Grpc.Core;

using Microsoft.Extensions.Logging;

namespace GomokuServer.Services
{
    public class GomokuService : Gomoku.Gomoku.GomokuBase
    {

        private readonly GamingServer _server;

        public GomokuService(GamingServer server)
        {
            _server = server;
        }

        public override async Task Play(IAsyncStreamReader<Request> requestStream, IServerStreamWriter<Reply> responseStream, ServerCallContext context)
        {
            await _server.Play(requestStream, responseStream);
        }
    }
}
