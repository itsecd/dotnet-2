using Lab2.Model;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2
{
    public class PongGameHub : Hub
    {
        private readonly ILogger<PongGameHub> _logger;

        public static ConcurrentDictionary<string, Player> UsersInLobby = new ConcurrentDictionary<string, Player>();

        public PongGameHub(ILogger<PongGameHub> logger)
        {
            _logger = logger;
        }

        public async Task Login(string player, string position)
        {
            if (UsersInLobby.Count > 2) return;
            _logger.LogInformation($"Login: {_logger}", player);
            UsersInLobby.AddOrUpdate(Context.ConnectionId, new Player(Context.ConnectionId, player, position), (key, oldPlayer)
                => new Player(key, player, position));

            await Clients.All.SendAsync("GetConnectedUsers",
                UsersInLobby.Select(x => new Player(x.Key, x.Value.UserName, x.Value.PlayerPosition)).ToList());

            if (UsersInLobby.Count > 1 && UsersInLobby.Count(u => u.Value.PlayerPosition != "") == 2)
            {
                var task = Task.Run(async () =>
                {
                    for (var i = 5; i >= 0; i--)
                    {
                        await Task.Delay(1000);
                        _logger.LogInformation("StartGameCountdown" + i);
                        await Clients.All.SendAsync("StartGameCountdown", i);
                    }
                });
                await Task.WhenAll(task);
                _logger.LogInformation("StartGame");
                await Clients.All.SendAsync("StartGame");
            }
        }

        public async Task GetConnectedPlayers()
        {
            _logger.LogInformation($"GetConnectedPlayers {UsersInLobby.Count}");
            await Clients.All.SendAsync("GetConnectedPlayers",
                UsersInLobby.Select(x => new Player(x.Key, x.Value.UserName, x.Value.PlayerPosition)).ToList());

        }

        public async Task GetTakenGameSide()
        {
            _logger.LogInformation($"GetTakenGameSide {UsersInLobby.Count}");
            await Clients.Caller.SendAsync("GetTakenGameSide",
                UsersInLobby.Select(x => x.Value.PlayerPosition));
        }

        public async Task MakeGoals(GameScore gameScore)
        {
            if (gameScore.RightScore >= 10 || gameScore.LeftScore >= 10)
            {
                await Clients.All.SendAsync("FinishGame", gameScore);
                foreach (var player in UsersInLobby)
                {
                    player.Value.PlayerPosition = "";
                }
                await GetConnectedPlayers();
                return;
            }
        }

        public async Task UpdateGamePosition(BallPosition ballPosition)
        {
            _logger.LogInformation($"UpdateGamePosition");
            await Clients.Others.SendAsync("UpdateGamePosition", ballPosition);
        }

        public async Task UpdatePlayerPosition(PlayerPosition playerPosition)
        {
            _logger.LogInformation($"UpdatePlayerPosition");
            await Clients.Others.SendAsync("UpdatePlayerPosition", playerPosition);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            UsersInLobby.Remove(Context.ConnectionId, out _);
            Clients.All.SendAsync("GetConnectedUsers", UsersInLobby.Select(x =>
                    new Player(x.Key, x.Value.UserName, x.Value.PlayerPosition)).ToList());
            Clients.Others.SendAsync("GameLeft");
            return base.OnDisconnectedAsync(exception);
        }
    }
}
