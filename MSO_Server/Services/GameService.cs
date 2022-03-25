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

        public override Task<RoomInfo> CreateRoom(CreateInfo request, ServerCallContext context)
        {
            _logger.LogDebug("User '{username}' created room, {count} is max players count.", request.PlayerName, request.PlayersMax);
            return Task.FromResult(new RoomInfo
            {
                RoomId = 111
            });
        }

        public override Task<ConnectionStatus> JoinRoom(RoomRequest request, ServerCallContext context)
        {
            _logger.LogDebug("User '{username}' connected to room #{room_id}.", request.PlayerName, request.RoomId);
            return Task.FromResult(new ConnectionStatus
            {
                Connected = true
            });
        }

        public override Task<ConnectionStatus> LeaveRoom(RoomRequest request, ServerCallContext context)
        {
            _logger.LogDebug("User '{username}' disconnected from room #{room_id}.", request.PlayerName, request.RoomId);
            return Task.FromResult(new ConnectionStatus
            {
                Connected = false
            });
        }
    }
}