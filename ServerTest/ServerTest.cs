using System;
using Xunit;
using MinesweeperServer.Database;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ServerTest
{
    public class RepositoryTest
    {
        [Fact]
        public void PlayerAddTest()
        {
            GameRepository repo = new(null);
            Assert.True(repo.TryAdd("username"));
            Assert.False(repo.TryAdd("username"));
        }
        [Fact]
        public void ScoreTest()
        {
            var configurationRoot = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            GameRepository repo = new(configurationRoot);
            repo.TryAdd("username");
            Assert.True(repo.CalcScore("username", "win"));
            Assert.True(repo.CalcScore("username", "win"));
            Assert.Equal(20, repo["username"].TotalScore);
            Assert.Equal(2, repo["username"].WinStreak);
            Assert.Equal(2, repo["username"].WinCount);
            Assert.Equal(0, repo["username"].LoseCount);
            Assert.True(repo.CalcScore("username", "lose"));
            Assert.Equal(8, repo["username"].TotalScore);
            Assert.Equal(0, repo["username"].WinStreak);
            Assert.Equal(2, repo["username"].WinCount);
            Assert.Equal(1, repo["username"].LoseCount);
        }
    }
    public class NetworkTest
    {
        [Fact]
        public void ConnectionTest()
        {
            GameNetwork network = new();
            Assert.True(network.Join("username", null));
            Assert.False(network.Join("username", null));
            Assert.True(network.IsConnected("username"));
            Assert.True(network.Leave("username"));
            Assert.False(network.Leave("username"));
            Assert.False(network.IsConnected("username"));
        }
        [Fact]
        public void StatesTest()
        {
            GameNetwork network = new();
            network.Join("username", null);
            network.Join("destroyer", null);
            Assert.True(network.AllStates("lobby"));
            network.DeclareWin("destroyer");
            Assert.Equal("win", network.GetPlayerState("destroyer"));
            Assert.Equal("lose", network.GetPlayerState("username"));

            network.SetPlayerState("username", "lobby");
            Assert.Equal("lobby", network.GetPlayerState("username"));
        }
    }
}
