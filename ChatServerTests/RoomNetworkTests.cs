using Grpc.Core;
using Moq;
using System;
using Xunit;

namespace ChatServer.Tests
{
    public class RoomNetworkTests
    {



        [Fact]
        public void JoinTest()
        {
            var testRoom = new RoomNetwork();
            testRoom.Join("user1", null);
            Assert.True(testRoom.Online.ContainsKey("user1"));
        }

        [Fact]
        public void AddUserTest()
        {
            var testRoom = new RoomNetwork();
            testRoom.AddUser("user1");
            testRoom.AddUser("user2");
            testRoom.AddUser("user3");
            var actual = testRoom.Users.ToArray();
            Assert.True(Array.Exists(actual, x => x.Name == "user1"));
            Assert.True(Array.Exists(actual, x => x.Name == "user2"));
            Assert.True(Array.Exists(actual, x => x.Name == "user3"));
        }

        [Fact]
        public async void BroadcastMessageTest()
        {
            var testRoom = new RoomNetwork();

            var message1 = new Message { User = "user1", Text = "Hello", Command = "message" };
            var message2 = new Message { User = "user2", Text = "Priveet", Command = "message" };
            var mock = new Mock<IServerStreamWriter<Message>>();
            testRoom.Join("user1", mock.Object);
            testRoom.Join("user2", mock.Object);
            testRoom.Join("user3", mock.Object);
            await testRoom.BroadcastMessage(message1);
            await testRoom.BroadcastMessage(message2);
            Assert.Equal(2, testRoom.History.Count);
            mock.Verify(x => x.WriteAsync(message1), Times.Exactly(3));
            mock.Verify(x => x.WriteAsync(message2), Times.Exactly(3));

        }

        [Fact]
        public void DisconnectTest()
        {
            var testRoom = new RoomNetwork();
            testRoom.Join("user1", null);
            testRoom.Join("user2", null);
            testRoom.Join("user3", null);
            testRoom.Disconnect("user1");
            Assert.True(!testRoom.Online.ContainsKey("user1"));
            Assert.True(testRoom.Online.ContainsKey("user2"));
            Assert.True(testRoom.Online.ContainsKey("user3"));
        }
    }
}