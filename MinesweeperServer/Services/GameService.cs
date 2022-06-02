using System;
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
            _logger.LogDebug("GameService constructed!");
        }
        public override async Task Join(IAsyncStreamReader<GameMessage> requestStream, IServerStreamWriter<GameMessage> responseStream, ServerCallContext context)
        {
            if (!await requestStream.MoveNext()) return;
            string playerName = requestStream.Current.Name;                              // save players name
            _logger.LogInformation("[{username}] присоединился к комнате!", playerName); // log
            if (_players.TryAddPlayer(playerName)) await _players.DumpAsync();           // save player, if new
            _users.Join(playerName, responseStream);                                     // join player to the server


            GameMessage message = new();
            do // MAIN CYCLE
            {
                while (_users.GetPlayerState(playerName) != "ready") // LOBBY
                {
                    await requestStream.MoveNext();
                    message = requestStream.Current;
                    switch (message.Text)
                    {
                        case "players":
                            foreach (string username in _users.GetPlayers)
                            {
                                Console.WriteLine($"send {username}'s stats");
                                await _users.SendPlayer(playerName, username, _players[username]);
                            }
                            await _users[playerName].Channel.WriteAsync(new GameMessage{ Text = "end" });
                            break;
                        case "ready":
                            _users.SetPlayerState(playerName, "ready");
                            _logger.LogInformation("[{username}] готов", playerName);
                            break;
                        case "leave":
                            _users.Leave(playerName);
                            _logger.LogInformation("[{username}] покинул комнату", playerName);
                            return;
                        default:
                            _logger.LogWarning("Unknown command: {command}", message.Text);
                            break;
                    }
                }


                while (!_users.AllStates("ready"));                                  // wait for all users to be ready
                await responseStream.WriteAsync(new GameMessage { Text = "start" }); // send starting signal
                _logger.LogInformation("[{username}] приступил к игре!", playerName);      // log


                while (_users.GetPlayerState(playerName) != "lobby") // GAME
                {
                    await requestStream.MoveNext();
                    message = requestStream.Current;
                    switch (message.Text)
                    {
                        case "players":
                            foreach (string player_name in _users.GetPlayers)
                                await _users.SendPlayer(playerName, player_name, _players[player_name]);
                            await _users[playerName].Channel.WriteAsync(new GameMessage{Text="end"});
                            break;
                        case "leave":
                            _users.Leave(playerName);
                            _logger.LogInformation("[{username}] покинул комнату", playerName);
                            return;
                        case "win":
                            _users.SetPlayerState(playerName, "lobby");                                               // send player to lobby
                            _players.CalcScore(playerName, "win");                                                    // calculate score
                            await _users.Broadcast(new GameMessage { Text = playerName, State = "win" }, playerName); // broadcast winner's name
                            _logger.LogInformation("[{username}] выиграл", playerName);                                     // log
                            break;
                        case "lose":
                            _users.SetPlayerState(playerName, "lobby");                                                // send player to lobby
                            _players.CalcScore(playerName, "lose");                                                    // calculate score
                            await _users.Broadcast(new GameMessage { Text = playerName, State = "lose" }, playerName); // broadcast loser's name
                            _logger.LogInformation("[{username}] проиграл", playerName);                                     // log
                            break;
                        default:
                            _logger.LogWarning("Unknown command: {command}", message.Text);
                            break;
                    }
                }
                await _players.DumpAsync();
            } while (_users.IsConnected(playerName));
        }
    }
}