using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text.Json;
using System.IO;
using System;

namespace MSO_Server.Data
{
    /// <summary>Класс для хранения информации об игроке.</summary>
    public class Player
    {
        public string Name;
        public int TotalScore = 0;
        public int WinCount = 0;
        public int LoseCount = 0;
        public int WinStreak = 0;
    }

    /// <summary>Репозиторий для хранения списка игроков.</summary>
    public class PlayerRepository
    {
        private readonly ConcurrentBag<Player> _players1 = new();
        private List<Player> _players;
        public Player this[string name] { get => _players.Find(x => x.Name == name); }
        public int Count { get => _players.Count; }

        public PlayerRepository() => _players = new();

        /// <summary>Добавление игрока в список, если не найден.</summary>
        /// <returns>'true' если игрок успешно добавлен, иначе 'false'</returns>
        public bool Add(string name)
        {
            _players1.Add(new Player{Name = name});
            if (!Exists(name))
            {
                _players.Add(new Player{Name = name});
                return true;
            }
            return false;
        }

        /// <summary>Удаление игрока из списка, если найден.</summary>
        /// <returns>true если игрок успешно удален, иначе 'false'</returns>
        public bool Delete(string name)
        {
            if (Exists(name))
            {
                var delInd = _players.FindIndex(x => x.Name == name);
                _players.RemoveAt(delInd);
                return true;
            }
            return false;
        }

        /// <summary>Существует ли игрок с данным именем в списке.</summary>
        public bool Exists(string name)
        {
            return _players.Exists(x => x.Name == name);
        }

        /// <summary>Загрузка списка игроков.</summary>
        public void Load()
        {
            if (File.Exists("players.json"))
            {
                string jsonString = File.ReadAllText("players.json");
                _players = JsonSerializer.Deserialize<List<Player>>(jsonString);
            }
        }

        /// <summary>Сохранение списка игроков.</summary>
        public void Dump()
        {
            string jsonString = JsonSerializer.Serialize<List<Player>>(_players, new JsonSerializerOptions { IncludeFields = true, WriteIndented = true });
            File.WriteAllText("players.json", jsonString);
        }
    }
}