using Xunit;
using MinesweeperServer.Database;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Collections.Generic;

namespace ServerTest
{
    public class RepositoryTest
    {
        [Fact]
        public void PlayerAddTest()
        {
            GameRepository repo = new(null);
            Assert.True(repo.TryAddPlayer("username"));
            Assert.False(repo.TryAddPlayer("username"));
        }
        [Fact]
        public void CalcScoreTest()
        {
            var configurationRoot = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            GameRepository repo = new(configurationRoot);
            repo.TryAddPlayer("username");
            Assert.True(repo.CalcScore("username", "win"));
            Assert.Equal(10, repo["username"].TotalScore);
            Assert.Equal(1, repo["username"].WinStreak);
            Assert.Equal(1, repo["username"].WinCount);
            Assert.Equal(0, repo["username"].LoseCount);
            Assert.True(repo.CalcScore("username", "lose"));
            Assert.Equal(0, repo["username"].TotalScore);
            Assert.Equal(0, repo["username"].WinStreak);
            Assert.Equal(1, repo["username"].WinCount);
            Assert.Equal(1, repo["username"].LoseCount);
        }
        [Fact]
        public void LoadTest()
        {
            string jsonString = "{ \"player0\": { \"TotalScore\": 20, \"WinCount\": 2, \"LoseCount\": 1, \"WinStreak\": 2 } }";
            File.WriteAllText("players.json", jsonString);
            var configurationRoot = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            GameRepository repo = new(configurationRoot);
            repo.Load();
            var controlPlayer = new Player{TotalScore = 20, WinCount = 2, LoseCount = 1, WinStreak = 2};
            Assert.Equal(controlPlayer.TotalScore, repo["player0"].TotalScore);
            Assert.Equal(controlPlayer.WinCount, repo["player0"].WinCount);
            Assert.Equal(controlPlayer.LoseCount, repo["player0"].LoseCount);
            Assert.Equal(controlPlayer.WinStreak, repo["player0"].WinStreak);
        }
        [Fact]
        public async void DumpAsyncTest()
        {
            var configurationRoot = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            GameRepository repo = new(configurationRoot);
            repo.TryAddPlayer("user1");
            repo.CalcScore("user1", "win");
            await repo.DumpAsync();
            List<string> controlList = new();
            controlList.Add("{\n  \"user1\": {\n    \"TotalScore\": 10,\n    \"WinCount\": 1,\n    \"LoseCount\": 0,\n    \"WinStreak\": 1\n  }\n}");
            controlList.Add("{\r\n  \"user1\": {\r\n    \"TotalScore\": 10,\r\n    \"WinCount\": 1,\r\n    \"LoseCount\": 0,\r\n    \"WinStreak\": 1\r\n  }\r\n}");
            Assert.Contains(File.ReadAllTextAsync("players.json").Result, controlList);
        }
    }
}
