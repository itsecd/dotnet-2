using System;
using Xunit;
using MinesweeperServer.Database;
using Microsoft.Extensions.Configuration;

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
            GameRepository repo = new(null);
            repo.TryAdd("username");
            Assert.True(repo.CalcScore("username", "win"));
            Assert.True(repo.CalcScore("username", "win"));
            Assert.Equal(repo["username"].TotalScore, 20);
            Assert.Equal(repo["username"].WinStreak, 2);
            Assert.Equal(repo["username"].WinCount, 2);
            Assert.Equal(repo["username"].LoseCount, 0);
            Assert.True(repo.CalcScore("username", "lose"));
            Assert.Equal(repo["username"].TotalScore, 8);
            Assert.Equal(repo["username"].WinStreak, 0);
            Assert.Equal(repo["username"].WinCount, 2);
            Assert.Equal(repo["username"].LoseCount, 1);
        }
    }
}
