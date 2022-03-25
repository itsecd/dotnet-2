using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace MSO_Server
{
    public class GameService : GameNetwork.GameNetworkBase
    {
        private readonly ILogger<GameService> _logger;
        public GameService(ILogger<GameService> logger)
        {
            _logger = logger;
        }

        public override Task<CreateResponse> CreateRoom(CreateRequest request, ServerCallContext context)
        {
            _logger.LogInformation("User {User} created room with {Count} max players count.", request.PlayerName, request.MaxPlayers);
            // return base.CreateRoom(request, context);
            return Task.FromResult(new CreateResponse
            {
                RoomId = 123
            });
        }
    }
}