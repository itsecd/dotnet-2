using ChatServer.Converters;
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChatServer.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private ConcurrentDictionary<string, RoomNetwork> _current = new();

        public ConcurrentDictionary<string, RoomNetwork> Rooms { get { return _current; } set { _current = value; } }
        public void WriteToFile()
        {
            foreach (var (Key, Value) in _current)
            {
                var jsonString = JsonSerializer.Serialize(Value, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(Key + ".json", jsonString);
            }

        }

        public void ReadFile(string nameRoom)
        {

            if (_current.ContainsKey(nameRoom))
            {
                var jsonString = File.ReadAllText(nameRoom + ".json");
                _current[nameRoom] = JsonSerializer.Deserialize<RoomNetwork>(jsonString);
            }

        }

        public async Task ReadAsync(string nameRoom)
        {
            if (IsRoomExists(nameRoom))
            {
                await using var stream = File.Open(nameRoom + ".json", FileMode.Open);
                var serializeOptions = new JsonSerializerOptions
                {
                    Converters =
                    {
                        new UsersBagJsonConverter()
                    }
                };
                if (_current.ContainsKey(nameRoom))
                {
                    var tmp = await JsonSerializer.DeserializeAsync<RoomNetwork>(stream, serializeOptions);
                    _current[nameRoom].Users = tmp.Users;
                    _current[nameRoom].History = tmp.History;
                }      
                else
                    AddRoom(nameRoom, await JsonSerializer.DeserializeAsync<RoomNetwork>(stream, serializeOptions));
                
            }

        }
        public async Task WriteAsync()
        {
            foreach (var (Key, Value) in _current)
            {
                using FileStream streamMessage = File.Create(Key + ".json");
                await JsonSerializer.SerializeAsync<RoomNetwork>(streamMessage, Value, new JsonSerializerOptions { WriteIndented = true });
                await streamMessage.DisposeAsync();
            }

        }
        public string AddRoom(string nameRoom, RoomNetwork room)
        {
            _current.TryAdd(nameRoom, room);
            return nameRoom;
        }

        public bool IsRoomExists(string nameRoom) => File.Exists(nameRoom + ".json");
        public void RemoveRoom(string nameRoom) => _current.TryRemove(nameRoom, out _);

        public RoomNetwork FindRoom(string nameRoom) => _current[nameRoom];

    }
}

