using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace MSO_Server.Data
{
    /// <summary>Класс для хранения информации об игроке.</summary>
    public class PlayerInfo
    {
        [JsonInclude]
        public int TotalScore = 0;
        [JsonInclude]
        public int WinCount = 0;
        [JsonInclude]
        public int LoseCount = 0;
        [JsonInclude]
        public int WinStreak = 0;
    }

    /// <summary>Репозиторий для хранения списка игроков.</summary>
    public class PlayerRepository
    {
        private ConcurrentDictionary<string, PlayerInfo> _players;
        public PlayerInfo this[string name] { get => _players[name]; }
        public int Count { get => _players.Count; }

        public PlayerRepository() => _players = new();

        /// <summary>Добавление игрока в список, если не найден.</summary>
        /// <returns>'true' если игрок успешно добавлен, иначе 'false'</returns>
        public bool Add(string name)
        {
            if (!Exists(name))
                return _players.TryAdd(name, new PlayerInfo());
            return false;
        }

        /// <summary>Удаление игрока из списка, если найден.</summary>
        /// <returns>true если игрок успешно удален, иначе 'false'</returns>
        public bool Delete(string name)
        {
            PlayerInfo res;
            return _players.TryRemove(name, out res);
        }

        /// <summary>Существует ли игрок с данным именем в списке.</summary>
        public bool Exists(string name) => _players.ContainsKey(name);

        /// <summary>Загрузка списка игроков.</summary>
        public void Load()
        {
            if (File.Exists("players.json"))
            {
                string jsonString = File.ReadAllText("players.json");
                _players = JsonSerializer.Deserialize<ConcurrentDictionary<string, PlayerInfo>>(jsonString);
            }
        }

        /// <summary>Сохранение списка игроков.</summary>
        public void Dump()
        {
            string jsonString = JsonSerializer.Serialize<ConcurrentDictionary<string, PlayerInfo>>(_players, new JsonSerializerOptions { IncludeFields = true, WriteIndented = true });
            File.WriteAllText("players.json", jsonString);
        }
    }
}