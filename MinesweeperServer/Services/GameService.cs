using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace MinesweeperServer
{
    public class GameService : Minesweeper.MinesweeperBase
    {
        private readonly ILogger<GameService> _logger;
        private readonly GameDatabase _data;
        public GameService(ILogger<GameService> logger, GameDatabase data)
        {
            _logger = logger;
            _data = data;
            _data.Load();
        }

        public override async Task Join(IAsyncStreamReader<PlayerMessage> requestStream, IServerStreamWriter<ServerMessage> responseStream, ServerCallContext context)
        {
            if (!await requestStream.MoveNext()) return;
            var initMessage = requestStream.Current;
            string playerName = initMessage.Name;
            _logger.LogTrace("[{username}] присоединился к комнате", playerName);
            // add player if new
            if (_data.TryAdd(playerName))
                await _data.DumpAsync();
            // join player
            _data.Join(playerName, responseStream);
            do
            {
                // lobby
                PlayerMessage message = new();
                while (_data.GetPlayerState(playerName) != "ready")
                {
                    await requestStream.MoveNext();
                    message = requestStream.Current;
                    switch (message.Text)
                    {
                        case "players":
                            await _data.SendPlayers(playerName);
                            break;
                        case "ready":
                            _data.Ready(playerName);
                            _logger.LogTrace("[{username}] готов", playerName);
                            break;
                        case "leave":
                            _data.Leave(playerName);
                            _logger.LogTrace("[{username}] покинул комнату", playerName);
                            return;
                        default:
                            break;
                    }
                }
                while (!_data.AllStates("ready")) ;
                await responseStream.WriteAsync(new ServerMessage { Text = "start" });
                // in game
                _logger.LogTrace("[{username}] приступил к игре", playerName);
                while (_data.GetPlayerState(playerName) != "lobby")
                {
                    await requestStream.MoveNext();
                    message = requestStream.Current;
                    switch (message.Text)
                    {
                        case "players":
                            await _data.SendPlayers(playerName);
                            break;
                        case "leave":
                            _data.Leave(playerName);
                            _logger.LogTrace("[{username}] покинул комнату", playerName);
                            return;
                        case "win":
                            _data.SetPlayerState(playerName, "win");
                            _data.CalcScore(playerName);
                            await _data.Broadcast(new ServerMessage { Text = playerName, State = "win" }, playerName);
                            _logger.LogTrace("[{username}] выиграл", playerName);
                            break;
                        case "lose":
                            _data.SetPlayerState(playerName, "lose");
                            _data.CalcScore(playerName);
                            await _data.Broadcast(new ServerMessage { Text = playerName, State = "lose" }, playerName);
                            _logger.LogTrace("[{username}] проиграл", playerName);
                            break;
                        default:
                            break;
                    }
                }
                await _data.DumpAsync();
            } while (_data.IsConnected(playerName));
        }
    }
}