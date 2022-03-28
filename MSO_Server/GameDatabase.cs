using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text.Json;
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
    /// <summary>Класс для хранения параметров комнаты и списка игроков.</summary>
    public class Room
    {
        public int PlayersMax { get; set; }
        public int PlayersCount => Players.Count;
        public int PlayersReady => (Players.Values.Where(val => val == -1)).Count();
        public ConcurrentDictionary<string, int> Players { get; set; }
        public int GameState { get; set; }

        /// <summary>Конструктор по умолчанию.</summary>
        public Room() => Players = new();
    }
    /// <summary>Класс для хранения игровых данных.</summary>
    public class GameDatabase
    {
        private readonly ConcurrentDictionary<string, IServerStreamWriter<ServerMessage>> _users = new();
        private ConcurrentDictionary<string, Player> _players = new();
        private ConcurrentDictionary<int, Room> _rooms = new();
        private readonly string pathPlayers = "players.json";
        private readonly string pathRooms = "rooms.json";

        public bool Join(int room_id, string name, IServerStreamWriter<ServerMessage> response)
        {
            if (!_rooms.ContainsKey(room_id) ||
                _rooms[room_id].PlayersCount == _rooms[room_id].PlayersMax ||
                _rooms[room_id].GameState != 0)
                return false;
            if (!_players.ContainsKey(name))
                _players.TryAdd(name, new Player());
            _rooms[room_id].Players.TryAdd(name, 0);
            _users.TryAdd(name, response);
            return true;
        }
        public bool Leave(int room_id, string name)
        {
            if (!_rooms.ContainsKey(room_id))
                return false;
            bool res1 = _rooms[room_id].Players.TryRemove(name, out var n);
            bool res2 = _users.TryRemove(name, out var s);
            return res1 && res2;
        }
        public int Create(string name, int players_max)
        {
            if (!_players.ContainsKey(name))
                _players.TryAdd(name, new Player());
            DateTime time = DateTime.Now;
            var rand = new Random(time.Hour * time.Minute * time.Second);
            int new_id = rand.Next(1, int.MaxValue);
            while (_rooms.ContainsKey(new_id))
                new_id = rand.Next(1, int.MaxValue);
            if (_rooms.TryAdd(new_id, new Room{PlayersMax = players_max}))
            {
                _rooms[new_id].Players.TryAdd(name, 0);
                return new_id;
            }
            return -1;
        }
        public bool GetReady(int room_id, string name)
        {
            if (_rooms.ContainsKey(room_id))
            {
                _rooms[room_id].Players[name] = -1;
                return true;
            }
            return false;
        }
        public void Dump()
        {
            string jsonString = JsonSerializer.Serialize(_players, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(pathPlayers, jsonString);
            jsonString = JsonSerializer.Serialize(_rooms, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(pathRooms, jsonString);
        }
        public void Load()
        {
            string jsonString;
            if (File.Exists(pathPlayers))
            {
                jsonString = File.ReadAllText(pathPlayers);
                _players = JsonSerializer.Deserialize<ConcurrentDictionary<string, Player>>(jsonString);
            }
            if (File.Exists(pathRooms))
            {
                jsonString = File.ReadAllText(pathRooms);
                _rooms = JsonSerializer.Deserialize<ConcurrentDictionary<int, Room>>(jsonString);
            }
        }
    }
}