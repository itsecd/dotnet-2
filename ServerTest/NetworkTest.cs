using Grpc.Core;
using MinesweeperServer;
using MinesweeperServer.Database;
using Xunit;
using Moq;
using System.Threading.Tasks;

namespace ServerTest
{
    public class NetworkTest
    {
        [Fact]
        public void JoinTwiceTest()
        {
            GameNetwork network = new();
            Assert.True(network.Join("username", null));
            Assert.False(network.Join("username", null));
        }
        [Fact]
        public void LeaveTwiceTest()
        {
            GameNetwork network = new();
            network.Join("username", null);
            Assert.True(network.Leave("username"));
            Assert.False(network.Leave("username"));
        }
        [Fact]
        public void ConnectionTest()
        {
            GameNetwork network = new();
            network.Join("username", null);
            Assert.True(network.IsConnected("username"));
            network.Leave("username");
            Assert.False(network.IsConnected("username"));
        }
        [Fact]
        public void PlayerStateTest()
        {
            GameNetwork network = new();
            network.Join("username", null);
            Assert.Equal("lobby", network.GetPlayerState("username"));
            network.SetPlayerState("username", "win");
            Assert.Equal("win", network.GetPlayerState("username"));
        }
        [Fact]
        public void AllStatesTest()
        {
            GameNetwork network = new();
            network.Join("username1", null);
            network.Join("username2", null);
            Assert.True(network.AllStates("lobby"));
            network.SetPlayerState("username1", "win");
            Assert.False(network.AllStates("lobby"));
        }
        [Fact]
        public async void SendPlayersTestAsync()
        {
            var mock = new Mock<IServerStreamWriter<ServerMessage>>();
            GameNetwork network = new();
            network.Join("user1", mock.Object);
            network.Join("user2", null);
            network.Join("user3", null);
            await network.SendPlayers("user1");
            mock.Verify(x => x.WriteAsync(new ServerMessage{Text = "user2", State = "lobby"}));
            mock.Verify(x => x.WriteAsync(new ServerMessage{Text = "user3", State = "lobby"}));
        }
        [Fact]
        public async void BroadcastTest()
        {
            var mock = new Mock<IServerStreamWriter<ServerMessage>>();
            GameNetwork network = new();
            network.Join("user1", mock.Object);
            network.Join("user2", mock.Object);
            await network.Broadcast(new ServerMessage{Text = "gg", State = "win"});
            mock.Verify(x => x.WriteAsync(new ServerMessage{Text = "gg", State = "win"}), Times.Exactly(2));
        }
    }
}