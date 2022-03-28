using System;
using System.IO;
using System.Linq;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MSO_Server.Data
{
    /// <summary>Класс для хранения параметров комнаты и списка игроков.</summary>
    public class Room
    {
        [JsonInclude]
        public int PlayersMax;
        public int PlayersCount => Players.Count;
        public int PlayersReady => (Players.Values.Where(val => val == -1)).Count();
        [JsonInclude]
        public ConcurrentDictionary<string, int> Players;
        [JsonInclude]
        public int GameState;

        /// <summary>Конструктор по умолчанию.</summary>
        public Room() => Players = new();

        /// <summary>Добавление игрока в комнату.</summary>
        public bool Join(string name) => Players.TryAdd(name, 0);

        /// <summary>Удаление игрока из комнаты.</summary>
        public bool Leave(string name)
        {
            int res;
            return Players.TryRemove(name, out res);
        }

        public void Ready(string name) => Players[name] = -1;
    }

    /// <summary>Репозиторий для хранения списка комнат.</summary>
    public class RoomRepository
    {
        private ConcurrentDictionary<int, Room> _rooms;
        public Room this[int id] => _rooms[id];
        public int Count => _rooms.Count;

        public RoomRepository() => _rooms = new();

        /// <summary>Создание новой комнаты.</summary>
        /// <returns>Номер новой комнаты.</returns>
        public int Add(int players_max)
        {
            // генерируем новый номер
            DateTime time = DateTime.Now;
            var rand = new Random(time.Hour * time.Minute * time.Second);
            int new_id = rand.Next();
            while (_rooms.ContainsKey(new_id))
                new_id = rand.Next();
            // создаем новую комнату
            var room = new Room();
            room.PlayersMax = players_max;
            // добавляем комнату в список
            _rooms.TryAdd(new_id, room);
            return new_id;
        }

        /// <summary>Удаление комнаты.</summary>
        public bool Delete(int room_id)
        {
            Room res;
            return _rooms.TryRemove(room_id, out res);
        }

        /// <summary>Существует ли комната с таким номером.</summary>
        public bool Exists(int id) => _rooms.ContainsKey(id);

        /// <summary>Загрузка списка комнат.</summary>
        public void Load()
        {
            if (File.Exists("rooms.json"))
            {
                string jsonString = File.ReadAllText("rooms.json");
                _rooms = JsonSerializer.Deserialize<ConcurrentDictionary<int, Room>>(jsonString);
            }
        }

        /// <summary>Сохранение списка комнат.</summary>
        public void Dump()
        {
            string jsonString = JsonSerializer.Serialize(_rooms, new JsonSerializerOptions { IncludeFields = true, WriteIndented = true });
            File.WriteAllText("rooms.json", jsonString);
        }
    }
}