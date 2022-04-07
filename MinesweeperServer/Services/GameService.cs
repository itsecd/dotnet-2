using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using MinesweeperServer.Database;

namespace MinesweeperServer
{
    public class GameService : Minesweeper.MinesweeperBase
    {
        private readonly ILogger<GameService> _logger;
        private readonly IGameRepository _players;
        private readonly IGameNetwork _users;

        public GameService(ILogger<GameService> logger, IGameRepository players, IGameNetwork users)
        {
            _logger = logger;
            _players = players;
            _users = users;
            _players.Load();
        }
        public override async Task Join(IAsyncStreamReader<PlayerMessage> requestStream, IServerStreamWriter<ServerMessage> responseStream, ServerCallContext context)
        {
            if (!await requestStream.MoveNext()) return;
            var initMessage = requestStream.Current;
            string playerName = initMessage.Name;
            _logger.LogTrace("[{username}] присоединился к комнате", playerName);
            // add player if new
            if (_players.TryAdd(playerName))
                await _players.DumpAsync();
            // join player
            _users.Join(playerName, responseStream);
            do
            {
                // lobby
                PlayerMessage message = new();
                while (_users.GetPlayerState(playerName) != "ready")
                {
                    await requestStream.MoveNext();
                    message = requestStream.Current;
                    switch (message.Text)
                    {
                        case "players":
                            await _users.SendPlayers(playerName);
                            break;
                        case "ready":
                            _users.SetPlayerState(playerName, "ready");
                            _logger.LogTrace("[{username}] готов", playerName);
                            break;
                        case "leave":
                            _users.Leave(playerName);
                            _logger.LogTrace("[{username}] покинул комнату", playerName);
                            return;
                        default:
                            break;
                    }
                }
                while (!_users.AllStates("ready")) ;
                await responseStream.WriteAsync(new ServerMessage { Text = "start" });
                // in game
                _logger.LogTrace("[{username}] приступил к игре", playerName);
                while (_users.GetPlayerState(playerName) != "lobby")
                {
                    await requestStream.MoveNext();
                    message = requestStream.Current;
                    switch (message.Text)
                    {
                        case "players":
                            await _users.SendPlayers(playerName);
                            break;
                        case "leave":
                            _users.Leave(playerName);
                            _logger.LogTrace("[{username}] покинул комнату", playerName);
                            return;
                        case "win":
                            _users.SetPlayerState(playerName, "lobby");
                            _players.CalcScore(playerName, "win");
                            await _users.Broadcast(new ServerMessage { Text = playerName, State = "win" }, playerName);
                            _logger.LogTrace("[{username}] выиграл", playerName);
                            break;
                        case "lose":
                            _users.SetPlayerState(playerName, "lobby");
                            _players.CalcScore(playerName, "lose");
                            await _users.Broadcast(new ServerMessage { Text = playerName, State = "lose" }, playerName);
                            _logger.LogTrace("[{username}] проиграл", playerName);
                            break;
                        default:
                            break;
                    }
                }
                await _players.DumpAsync();
            } while (_users.IsConnected(playerName));
        }
    }
}