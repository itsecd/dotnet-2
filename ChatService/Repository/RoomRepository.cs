using ChatService.Exceptions;
using ChatService.Models;
using Grpc.Core;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ChatService.Repository
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ConcurrentBag<Room> _rooms = new();
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
            if (!CheckUser(room, userName))
            {
                room.Users.Add(new User() { Name = userName, Connect = response });
            }
            else
            {
                throw new NameIsUseException();
            }
        }

        public List<string> GetAllUsers(string roomName)
        {
            Room room = FindRoom(roomName);
            List<string> users = new List<string>();
            foreach(User user in room.Users)
            {
                users.Add(user.Name);
            }
            return users;
        }

        public List<string> GetAllRooms()
        {
            List<string> rooms = new List<string>();
            foreach (Room room in _rooms)
            {
                rooms.Add(room.Name);
            }
            return rooms;
        }

        public void Disconnect(string roomName, string userName)
        {
            _rooms.First(f => f.Name == roomName).Users.First(x => x.Name == userName).Connect = null;
        }

        public async Task BroadcastMessage(Message message, string roomName)
        {
            Room room = FindRoom(roomName);
            foreach (User user in room.Users)
            {
                if (user.Connect != null && user.Name != message.UserName)
                {
                    await user.Connect.WriteAsync(message);
                }
            }
            _rooms.First(f => f.Name == roomName).Messages.Add(message.UserName + " : " + message.Text);
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
                if(room != null)
                {
                    _rooms.First(f => f.Name == name).Messages = room.Messages;
                }
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
                    XmlSerializer formatter = new(typeof(Room));
                    await using FileStream fileStream = new(room.Name + ".xml", FileMode.Create);
                    formatter.Serialize(fileStream, room);
                }
            }
            finally
            {
                SemaphoreSlim.Release();
            }
        }

        private bool CheckUser(Room room, string userName)
        {
            foreach (User user in room.Users)
            {
                if (user.Name == userName)
                {
                    return true;
                }
            }
            return false;
        }

        public Room FindRoom(string roomName)
        {
            if (_rooms.IsEmpty)
            {
                return null;
            }
            return _rooms.First(f => f.Name == roomName);
        }
    }
}
