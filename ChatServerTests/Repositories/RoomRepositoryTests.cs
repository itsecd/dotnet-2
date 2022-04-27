using System.Collections.Concurrent;
using System.IO;
using Xunit;

namespace ChatServer.Repositories.Tests
{

    public class RoomRepositoryTests
    {
        private static RoomRepository CreateTestRepository()
        {
            var user1 = new User("user1", 1);
            var user2 = new User("user2", 2);
            var user3 = new User("user3", 3);


            return new RoomRepository
            {
                Rooms = new ConcurrentDictionary<string, RoomNetwork>
                {
                    ["test1"] = new()
                    {
                        Users = new ConcurrentBag<User> { user1, user2 }
                    },
                    ["test2"] = new()
                    {
                        Users = new ConcurrentBag<User> { user1, user3 }
                    },
                    ["test3"] = new()
                    {
                        Users = new ConcurrentBag<User> { user2, user3 }
                    }
                }
            };
        }
        [Fact()]
        public async void WriteAsyncTest()
        {
            RoomRepository roomRepository = CreateTestRepository();
            await roomRepository.WriteAsync();
            Assert.True(File.Exists("test1.json"));
            Assert.True(File.Exists("test2.json"));
            Assert.True(File.Exists("test3.json"));
        }

        [Fact()]
        public async void ReadAsyncTest()
        {
            RoomRepository roomRepository = new();
            await roomRepository.ReadAsync("test1");
            await roomRepository.ReadAsync("test2");
            await roomRepository.ReadAsync("test3");
            Assert.True(roomRepository.Rooms.TryGetValue("test1", out _));
            Assert.True(roomRepository.Rooms.TryGetValue("test2", out _));
            Assert.True(roomRepository.Rooms.TryGetValue("test3", out _));
        }

        [Fact()]
        public void AddRoomTest()
        {
            var user4 = new User("user4", 4);
            var user5 = new User("user", 5);
            RoomRepository roomRepository = CreateTestRepository();
            RoomNetwork testRoom = new RoomNetwork { Users = new ConcurrentBag<User> { user4, user5 } };
            roomRepository.AddRoom("test4", testRoom);
            Assert.True(roomRepository.Rooms.ContainsKey("test4"));
            Assert.True(roomRepository.Rooms.TryGetValue("test4", out _));
        }

        [Fact()]
        public void IsRoomExistsTest()
        {
            RoomRepository roomRepository = CreateTestRepository();
            Assert.True(roomRepository.IsRoomExists("test1"));
        }

        [Fact()]
        public void RemoveRoomTest()
        {
            RoomRepository roomRepository = CreateTestRepository();
            roomRepository.RemoveRoom("test1");
            Assert.True(!roomRepository.Rooms.ContainsKey("test1"));
            Assert.True(!roomRepository.Rooms.TryGetValue("test1", out _));
        }

        [Fact()]
        public void FindRoomTest()
        {
            RoomRepository roomRepository = CreateTestRepository();
            var testRoom = roomRepository.FindRoom("test1");
            Assert.Equal(testRoom, roomRepository.Rooms["test1"]);
        }
    }
}