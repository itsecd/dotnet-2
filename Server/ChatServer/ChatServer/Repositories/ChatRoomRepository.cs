using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChatServer.Repositories
{
    public class ChatRoomRepository : IChatRoomRepository
    {
        private ConcurrentDictionary<int, ChatRoomUtils> _chatRooms = new();
        private readonly IConfiguration _config;

        public ChatRoomRepository(IConfiguration config)
        {
            _config = config;
        }

        public void WriteToFile()
        {
            var serializationRoom = JsonSerializer.Serialize(_chatRooms, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_config["chatroomfilepath"], serializationRoom);
        }

        public void ReadFile()
        {
            if (File.Exists(_config["chatroomfilepath"]))
            {
                string deserializationString = File.ReadAllText(_config["chatroomfilepath"]);
                _chatRooms = JsonSerializer.Deserialize<ConcurrentDictionary<int, ChatRoomUtils>>(deserializationString);
            }
        }

        public async Task ReadAsync()
        {
            if (File.Exists(_config["chatroomfilepath"]))
            {
                using FileStream stream = File.Open(_config["chatroomfilepath"], FileMode.Open);
                _chatRooms = await JsonSerializer.DeserializeAsync<ConcurrentDictionary<int, ChatRoomUtils>>(stream);
                await stream.DisposeAsync();
            }
        }
        public async Task WriteAsync()
        {
            using FileStream streamMessage = File.Create(_config["chatroomfilepath"]);
            await JsonSerializer.SerializeAsync<ConcurrentDictionary<int, ChatRoomUtils>>(streamMessage, _chatRooms, new JsonSerializerOptions { WriteIndented = true });
            await streamMessage.DisposeAsync();
        }
        public int AddRoom(int id, ChatRoomUtils room)
        {
            _chatRooms.TryAdd(id, room);
            return id;
        }
        

        public void RemoveRoom(int id) => _chatRooms.TryRemove(id, out _);

        public ChatRoomUtils FindRoom(int id) => _chatRooms[id];

    }
}

