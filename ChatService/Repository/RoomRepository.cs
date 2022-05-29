using ChatService.Exceptions;
using ChatService.Models;
using Grpc.Core;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ChatService.Repository
{
    public class RoomRepository : IRoomRepository
    {
        private ConcurrentBag<Room> _rooms = new();
        private static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);

        public void AddRoom(string name)
        {
            if (FindRoom(name) == null)
            {
                _rooms.Add(new Room() { Name = name });
            }
            else
            {
                throw new NameIsUseException();
            }

        }

        public void Join(string roomName, string userName, IServerStreamWriter<Message> response)
        {
            Room room = FindRoom(roomName);
            if(room.Users.Where(f => f.Name == userName).First() == null)
            {
                room.Users.Add(new User() { Name = userName, Connect = response });
            }
            else
            {
                throw new NameIsUseException();
            }
        }

        public void Disconnect(string roomName, string userName)
        {
            _rooms.Where(f => f.Name == roomName).First().Users.Where(x => x.Name == userName).First().Connect = null;
        }

        public async Task BroadcastMessage(Message message, string roomName, string userName)
        {
            Room room = FindRoom(roomName);
            foreach (User user in room.Users)
            {
                if(user.Connect != null)
                {
                    await user.Connect.WriteAsync(message);
                }
            }
        }

        public async Task ReadFileAsync(string name)
        {
            await SemaphoreSlim.WaitAsync();
            try
            {
                if (!File.Exists(name + ".xml"))
                {
                    AddRoom(name);
                }
                XmlSerializer formatter = new XmlSerializer(typeof(Room));
                await using FileStream fileStream = new FileStream(name + ".xml", FileMode.OpenOrCreate);
                Room room = (Room)formatter.Deserialize(fileStream);
                _rooms.Where(f => f.Name == name).First().Messages = room.Messages;
            }
            finally
            {
                SemaphoreSlim.Release();
            }
        }

        public async Task WriteFileAsync()
        {
            await SemaphoreSlim.WaitAsync();
            try
            {
                foreach (Room room in _rooms)
                {
                    XmlSerializer formatter = new XmlSerializer(typeof(Room));
                    await using FileStream fileStream = new FileStream(room.Name, FileMode.Create);
                    formatter.Serialize(fileStream, room);
                }
            }
            finally
            {
                SemaphoreSlim.Release();
            }
        }

        public Room FindRoom(string roomName)
        {
            return _rooms.Where(f => f.Name == roomName).First();
        }
    }
}
