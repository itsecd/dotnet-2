using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using MSO_Server.Data;
using Spectre.Console;
using System;

namespace MSO_Server
{
    public class GameService : GameNetwork.GameNetworkBase
    {
        private readonly ILogger<GameService> _logger;
        private readonly PlayerRepository _players;
        private readonly RoomRepository _rooms;
        public GameService(ILogger<GameService> logger, PlayerRepository players, RoomRepository rooms)
        {
            _logger = logger;
            _players = players;
            _rooms = rooms;
            _players.Load();
            _rooms.Load();
        }

        public override Task<RoomInfo> CreateRoom(CreateInfo request, ServerCallContext context)
        {
            try
            {
                // добавить нового игрока в список, если ник неизвестен
                // _players.Load();
                if (_players.Add(request.PlayerName))
                {
                    _players.Dump();
                    AnsiConsole.MarkupLine($"[bold yellow]New user: '{request.PlayerName}'[/]");
                }
                // создать новую комнату
                // _rooms.Load();
                int room_id = _rooms.Add(request.PlayersMax);
                _rooms[room_id].Join(request.PlayerName);
                _rooms.Dump();
                AnsiConsole.MarkupLine($"[bold yellow]New room: #{room_id}[/]");
                return Task.FromResult(new RoomInfo
                {
                    RoomId = room_id,
                    PlayersMax = request.PlayersMax
                });
            }
            catch (Exception e)
            {
                AnsiConsole.WriteException(e);
                return Task.FromResult(new RoomInfo { RoomId = 0, PlayersMax = 0 });
            }
        }

        public override Task<ConnectionStatus> JoinRoom(RoomRequest request, ServerCallContext context)
        {
            // добавить нового игрока в список, если ник неизвестен
            // _players.Load();
            if (_players.Add(request.PlayerName))
            {
                _players.Dump();
                AnsiConsole.MarkupLine($"[bold yellow]New user: '{request.PlayerName}'[/]");
            }
            // добавить игрока в комнату
            // _rooms.Load();
            if (_rooms.Exists(request.RoomId))
            {
                _rooms[request.RoomId].Join(request.PlayerName);
                _rooms.Dump();
                _logger.LogWarning("User '{username}' connected to room #{room_id}.", request.PlayerName, request.RoomId);
            }
            else
                _logger.LogError("Room #{room_id} does not exists!", request.RoomId);
            return Task.FromResult(new ConnectionStatus { Connected = true });
        }

        public override Task<ConnectionStatus> LeaveRoom(RoomRequest request, ServerCallContext context)
        {
            // убрать игрока из комнаты
            // _rooms.Load();
            if (_rooms.Exists(request.RoomId))
            {
                _rooms[request.RoomId].Leave(request.PlayerName);
                _rooms.Dump();
                _logger.LogWarning("User '{username}' disconnected from room #{room_id}.", request.PlayerName, request.RoomId);
            }
            else
                _logger.LogError("Room #{room_id} does not exists!", request.RoomId);
            return Task.FromResult(new ConnectionStatus { Connected = false });
        }

        public override Task StartGame(IAsyncStreamReader<PlayerState> requestStream, IServerStreamWriter<GameState> responseStream, ServerCallContext context)
        {
            return base.StartGame(requestStream, responseStream, context);
        }
    }
}