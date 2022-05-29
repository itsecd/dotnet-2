using Grpc.Core;
using SnakeServer;
using System.Threading.Tasks;

namespace Server.Services
{
    public class SnakeService:SnakeServer.Snake.SnakeBase
    {
        private readonly GameService _gameService;

        public override Task Play(IAsyncStreamReader<PlayerMessage> requestStream, IServerStreamWriter<ServerMessage> responseStream, ServerCallContext context)
        {

            return _gameService.Join(requestStream,responseStream);
        }
        
        public SnakeService(GameService gameService)
        {
            _gameService = gameService;
        } 
    }
}
