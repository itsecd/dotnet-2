using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;

namespace MinesweeperServer.Database
{
    public class GameNetwork : IGameNetwork
    {
        private readonly ConcurrentDictionary<string, User> _users = new();
        public User this[string name] => _users[name];
        
        public bool Join(string name, IServerStreamWriter<GameMessage> channel) => _users.TryAdd(name, new User { Channel = channel, State = "lobby" });
        public bool Leave(string name) => _users.TryRemove(name, out _);
        public bool IsConnected(string name) => _users.ContainsKey(name);
        public bool AllStates(string state) => _users.Values.All(x => x.State == state);
        public void SetPlayerState(string name, string state) => _users[name].State = state;
        public string GetPlayerState(string name) => _users[name].State;
        public async Task SendPlayer(string name, string username, Player stats)
        {
            await _users[name].Channel.WriteAsync(
                new GameMessage{
                    Name=username,
                    Text=$"{stats.WinCount}_{stats.LoseCount}_{stats.WinStreak}"
                    }
                );
        }
        public async Task Broadcast(GameMessage message, string name = "")
        {
            foreach (var player in _users.Where(x => x.Key != name))
            {
                await _users[player.Key].Channel.WriteAsync(message);
            }
        }
        public ICollection<string> GetPlayers => _users.Keys;
    }
}