using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Grpc.Core;

namespace MSO_Server
{
    /// <summary>Класс для хранения информации об игроке.</summary>
    public class Player
    {
        public int TotalScore { get; set; }
        public int WinCount { get; set; }
        public int LoseCount { get; set; }
        public int WinStreak { get; set; }
    }
    public class User
    {
        public IServerStreamWriter<ServerMessage> Channel;
        public string State;
    }
    /// <summary>Класс для хранения параметров комнаты и списка игроков.</summary>
    public class Room
    {
        public int PlayersMax { get; set; }
        public int PlayersCount => Players.Count;
        public int PlayersReady => (Players.Values.Where(val => val == -1)).Count();
        public ConcurrentDictionary<string, int> Players { get; set; }
        public string GameState { get; set; }

        /// <summary>Конструктор по умолчанию.</summary>
        public Room() => Players = new();
    }
    /// <summary>Класс для хранения игровых данных.</summary>
    public class GameDatabase
    {
        private readonly ConcurrentDictionary<string, User> _users = new();
        private ConcurrentDictionary<string, Player> _players = new();
        private readonly string pathPlayers = "players.json";

        public bool TryAdd(string name)
        {
            if (_players.ContainsKey(name))
                return false;
            return _players.TryAdd(name, new Player());
        }
        public bool Join(string name, IServerStreamWriter<ServerMessage> channel) => _users.TryAdd(name, new User { Channel = channel, State = "lobby" });
        public bool Leave(string name) => _users.TryRemove(name, out var s);
        public void Ready(string name) => _users[name].State = "ready";
        public void Dump()
        {
            string jsonString = JsonSerializer.Serialize(_players, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(pathPlayers, jsonString);
        }
        public void Load()
        {
            if (File.Exists(pathPlayers))
            {
                var jsonString = File.ReadAllText(pathPlayers);
                _players = JsonSerializer.Deserialize<ConcurrentDictionary<string, Player>>(jsonString);
            }
        }

        public async Task SendPlayers(string name)
        {
            foreach (var player in _users.Where(x => x.Key != name))
            {
                await _users[name].Channel.WriteAsync(new ServerMessage { Text = player.Key, State = _users[player.Key].State });
            }
        }

        public async Task Broadcast(ServerMessage message, string name = "")
        {
            foreach (var player in _users.Where(x => x.Key != name))
            {
                await _users[player.Key].Channel.WriteAsync(message);
            }
        }
    }
}