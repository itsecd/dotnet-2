using Grpc.Core;
using System.Threading.Tasks;

namespace RaceServer.Services
{
    public class EntryPointService : RaceService.RaceServiceBase
    {
        private readonly GameService _gameService;

        public EntryPointService(GameService gameService)
        {
            _gameService = gameService;
        }

        public override async Task Connect(
            IAsyncStreamReader<Request> requestStream,
            IServerStreamWriter<Event> responseStream,
            ServerCallContext context)
        {
            await _gameService.Connect(requestStream, responseStream);
        }
    }
}