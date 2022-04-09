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
}
