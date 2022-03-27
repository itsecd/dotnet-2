using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text.Json;
using System.IO;
using System;
using System.Text.Json.Serialization;

namespace MSO_Server.Data
{
    /// <summary>Класс для хранения параметров комнаты и списка игроков.</summary>
    public class Room
    {
        public int RoomId = 0;
        public int PlayersMax = 1;
        public int PlayersCount = 1;
        public int PlayersReady = 0;

        [JsonInclude]
        public List<string> Players;
        public int GameState = 0;

        /// <summary>Инициализация номера комнаты и списка игроков.</summary>
        public Room()
        {
            Players = new();
            DateTime time = DateTime.Now;
            var rand = new Random(time.Hour * time.Minute * time.Second);
            RoomId = rand.Next(1, 100000);
        }

        public void Join(string name)
        {
            Players.Add(name);
            PlayersCount++;
        }

        public void Leave(string name)
        {
            Players.Remove(name);
            PlayersCount--;
        }
    }

    /// <summary>Репозиторий для хранения списка комнат.</summary>
    public class RoomRepository
    {
        private List<Room> _rooms;
        public Room this[int id] { get => _rooms.Find(x => x.RoomId == id); }
        public int Count { get => _rooms.Count; }

        public RoomRepository() => _rooms = new();

        /// <summary>Создание новой комнаты.</summary><returns>Номер новой комнаты.</returns>
        public int Add(int players_max)
        {
            var room = new Room();
            room.PlayersMax = players_max;
            _rooms.Add(room);
            return room.RoomId;
        }

        /// <summary>Удаление комнаты.</summary>
        public void Delete(int room_id)
        {
            int indDel = _rooms.FindIndex(x => x.RoomId == room_id);
            _rooms.RemoveAt(indDel);
        }

        public bool Exists(int id) => _rooms.Exists(x => x.RoomId == id);

        /// <summary>Загрузка списка комнат.</summary>
        public void Load()
        {
            if (File.Exists("rooms.json"))
            {
                string jsonString = File.ReadAllText("rooms.json");
                _rooms = JsonSerializer.Deserialize<List<Room>>(jsonString);
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