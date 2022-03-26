using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using MSO_Server.Data;

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
            // добавить нового игрока в список, если ник неизвестен
            var players = new PlayerRepository();
            // players.Load();
            players.Add(request.PlayerName);
            players.Dump();
            _logger.LogWarning("New user: '{username}'", request.PlayerName);
            // создать новую комнату
            var rooms = new RoomRepository();
            // rooms.Load();
            int room_id = rooms.Add(request.PlayersMax);
            rooms.Dump();
            _logger.LogWarning("New room: '{room_id}'", room_id);
            return Task.FromResult(new RoomInfo
            {
                RoomId = room_id,
                PlayersMax = request.PlayersMax
            });
        }

        public override Task<ConnectionStatus> JoinRoom(RoomRequest request, ServerCallContext context)
        {
            // добавить нового игрока в список, если ник неизвестен
            var players = new PlayerRepository();
            // players.Load();
            if (!players.Exists(request.PlayerName))
            {
                players.Add(request.PlayerName);
                players.Dump();
                _logger.LogWarning("New user: '{username}'", request.PlayerName);
            }
            // добавить игрока в комнату
            var rooms = new RoomRepository();
            // rooms.Load();
            rooms[request.RoomId].Players.Add(request.PlayerName);
            rooms.Dump();
            _logger.LogWarning("User '{username}' connected to room #{room_id}.", request.PlayerName, request.RoomId);
            return Task.FromResult(new ConnectionStatus { Connected = true });
        }

        public override Task<ConnectionStatus> LeaveRoom(RoomRequest request, ServerCallContext context)
        {
            // добавить нового игрока в список, если ник неизвестен
            var players = new PlayerRepository();
            // players.Load();
            if (!players.Exists(request.PlayerName))
            {
                players.Add(request.PlayerName);
                players.Dump();
                _logger.LogWarning("New user: '{username}'", request.PlayerName);
            }
            // убрать игрока из комнаты
            var rooms = new RoomRepository();
            // rooms.Load();
            rooms[request.RoomId].Players.Remove(request.PlayerName);
            rooms.Dump();
            _logger.LogWarning("User '{username}' disconnected from room #{room_id}.", request.PlayerName, request.RoomId);
            return Task.FromResult(new ConnectionStatus { Connected = false });
        }

        public override Task StartGame(IAsyncStreamReader<PlayerState> requestStream, IServerStreamWriter<GameState> responseStream, ServerCallContext context)
        {
            return base.StartGame(requestStream, responseStream, context);
        }
    }
}