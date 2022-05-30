using ChatService;
using ChatService.Repository;
using Grpc.Core;
using Moq;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace ChatServiceTests
{
    public class RoomRepositoryTest
    {

        protected readonly ITestOutputHelper Output;

        public RoomRepositoryTest(ITestOutputHelper tempOutput)
        {
            Output = tempOutput;
        }

        [Fact]
        public async void WriteAsyncTest()
        {
            var roomRepository = new RoomRepository();
            roomRepository.AddRoom("roomToWrite");
            await roomRepository.WriteFileAsync();
            Assert.True(File.Exists("roomToWrite.xml"));
        }

        [Fact]
        public async void ReadAsyncTest()
        {
            var roomRepository = new RoomRepository();
            roomRepository.AddRoom("roomToRead");
            await roomRepository.WriteFileAsync();
            await roomRepository.ReadFileAsync("roomToRead");
            Assert.Contains("roomToRead", roomRepository.GetAllRooms());

        }

        [Fact]
        public void JoinTest()
        {
            var roomRepository = new RoomRepository();
            roomRepository.AddRoom("roomToJoin");
            roomRepository.Join("roomToJoin", "user1", new Mock<IServerStreamWriter<Message>>().Object);
            Assert.Contains("user1", roomRepository.GetAllUsers("roomToJoin"));
        }

        [Fact]
        public async void BroadcastMessageTest()
        {
            var roomRepository = new RoomRepository();
            roomRepository.AddRoom("roomToBroadcast");
            var message1 = new Message { UserName = "user1", Text = "Hello", RoomName = "roomToBroadcast" };
            var message2 = new Message { UserName = "user2", Text = "Priveet", RoomName = "roomToBroadcast" };
            var mock = new Mock<IServerStreamWriter<Message>>();
            roomRepository.Join("roomToBroadcast", "user1", mock.Object);
            roomRepository.Join("roomToBroadcast", "user2", mock.Object);
            roomRepository.Join("roomToBroadcast", "user3", mock.Object);
            await roomRepository.BroadcastMessage(message1, "roomToBroadcast");
            await roomRepository.BroadcastMessage(message2, "roomToBroadcast");
            mock.Verify(x => x.WriteAsync(message1), Times.Exactly(2));
            mock.Verify(x => x.WriteAsync(message2), Times.Exactly(2));

        }

        [Fact]
        public void AddRoomTest()
        {
            var roomRepository = new RoomRepository();
            roomRepository.AddRoom("room");
            Assert.Contains("room", roomRepository.GetAllRooms());
        }
    }
}
