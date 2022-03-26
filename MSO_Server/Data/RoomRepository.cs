using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using System;

namespace MSO_Server.Data
{
    public class Room
    {
        public int RoomId = 0;
        public int PlayersMax = 1;
        public int PlayersCount = 1;
        public int PlayersReady = 0;
        public List<string> Players;
        public int GameState = 0;

        ///<summary>Initialize room id and list of players.</summary>
        public Room()
        {
            Players = new();
            DateTime time = DateTime.Now;
            var rand = new Random(time.Hour * time.Minute * time.Second);
            RoomId = rand.Next(1, 100000);
        }
    }

    public class RoomRepository
    {
        private List<Room> _rooms;
        public Room this[int id] { get => _rooms.Find(x => x.RoomId == id); }
        public int Count { get => _rooms.Count; }

        public RoomRepository() => _rooms = new();

        ///<summary>Add new room.</summary><returns>Id of new room.</returns>
        public int Add(int players_max)
        {
            var room = new Room();
            room.PlayersMax = players_max;
            _rooms.Add(room);
            return room.RoomId;
        }

        public void Delete(int room_id)
        {
            int indDel = _rooms.FindIndex(x => x.RoomId == room_id);
            _rooms.RemoveAt(indDel);
        }

        public void Load()
        {
            string jsonString = File.ReadAllText("rooms.json");
            _rooms = JsonSerializer.Deserialize<List<Room>>(jsonString);
        }

        public void Dump()
        {
            string jsonString = JsonSerializer.Serialize(_rooms, new JsonSerializerOptions { IncludeFields = true, WriteIndented = true });
            File.WriteAllText("rooms.json", jsonString);
        }
    }
}