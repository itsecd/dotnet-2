using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using System;

namespace MSO_Server.Data
{
    public class Player
    {
        public string Name;
        public int TotalScore = 0;
        public int WinCount = 0;
        public int LoseCount = 0;
        public int WinStreak = 0;

        public Player()
        {
            Name = "username";
        }

        public Player(string name)
        {
            Name = name;
        }
    }

    public class PlayerRepository
    {
        public List<Player> _players;
        public Player this[string name] { get => _players.Find(x => x.Name == name); }
        public int Count { get => _players.Count; }

        public PlayerRepository() => _players = new();

        public void Add(string name)
        {
            _players.Add(new Player(name));
        }

        public void Delete(string name)
        {
            var delInd = _players.FindIndex(x => x.Name == name);
            _players.RemoveAt(delInd);
        }

        public bool Exists(string name)
        {
            return _players.Exists(x => x.Name == name);
        }

        public void Load()
        {
            string jsonString = File.ReadAllText("players.json");
            _players = JsonSerializer.Deserialize<List<Player>>(jsonString);
        }

        public void Dump()
        {
            string jsonString = JsonSerializer.Serialize<List<Player>>(_players, new JsonSerializerOptions { IncludeFields = true, WriteIndented = true });
            File.WriteAllText("players.json", jsonString);
        }
    }
}