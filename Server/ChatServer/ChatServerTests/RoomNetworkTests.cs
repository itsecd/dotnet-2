using Xunit;
using ChatServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Grpc.Core;
using Moq;

namespace ChatServer.Tests
{
    public class RoomNetworkTests
    {

        

        [Fact()]
        public void JoinTest()
        {
            var testRoom = new RoomNetwork();
            var mock = new Mock<IServerStreamWriter<Message>>();
            testRoom.Join("user1", mock.Object);
            Assert.True(testRoom.Online.ContainsKey("user1"));
        }

        [Fact()]
        public void AddUserTest()
        {
            var testRoom = new RoomNetwork();
            testRoom.AddUser("user1");
            testRoom.AddUser("user2");
            testRoom.AddUser("user3");
            Assert.Equal(3, testRoom.Users.Count());
        }

        [Fact()]
        public async void BroadcastMessageTest()
        {
            var testRoom = new RoomNetwork();

            var message1 = new Message { User = "user1", Text = "Hello", Command = "message"};
            var message2 = new Message { User = "user2", Text = "Priveet", Command = "message" };
            var mock1 = new Mock<IServerStreamWriter<Message>>();
            var mock2 = new Mock<IServerStreamWriter<Message>>();
            var mock3 = new Mock<IServerStreamWriter<Message>>();
            testRoom.Join("user1", mock1.Object);
            testRoom.Join("user2", mock2.Object);
            testRoom.Join("user3", mock3.Object);
            await testRoom.BroadcastMessage(message1);
            await testRoom.BroadcastMessage(message2);
            Assert.Equal(2, testRoom.History.Count);
            
        }

        [Fact()]
        public void DisconnectTest()
        {
            var testRoom = new RoomNetwork();
            var mock1 = new Mock<IServerStreamWriter<Message>>();
            var mock2 = new Mock<IServerStreamWriter<Message>>();
            var mock3 = new Mock<IServerStreamWriter<Message>>();
            testRoom.Join("user1", mock1.Object);
            testRoom.Join("user2", mock2.Object);
            testRoom.Join("user3", mock3.Object);
            testRoom.Disconnect("user1");
            Assert.True(!testRoom.Online.ContainsKey("user1"));
            Assert.True(testRoom.Online.ContainsKey("user2"));
            Assert.True(testRoom.Online.ContainsKey("user3"));
        }
    }
}