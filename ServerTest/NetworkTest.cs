using Grpc.Core;
using MinesweeperServer;
using MinesweeperServer.Database;
using Xunit;

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
        public void SendPlayersTest()
        {
            GameNetwork network = new();
            network.Join("user1", null);
            network.Join("user2", null);
            network.Join("user3", null);
        }
    }
}