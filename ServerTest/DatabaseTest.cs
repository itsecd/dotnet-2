using System;
using Xunit;
using MinesweeperServer;

namespace ServerTest
{
    public class DatabaseTest
    {
        [Theory]
        [InlineData("DimaDivan")]
        [InlineData("HikariIrai")]
        [InlineData("Heroillidan")]
        public void TotalTest(string username)
        {
            GameDatabase db = new(null);
            // player test
            Assert.True(db.TryAdd(username));
            Assert.False(db.TryAdd(username));
            // network test
            Assert.False(db.IsConnected(username));
            Assert.True(db.Join(username, null));
            Assert.True(db.IsConnected(username));
            // states test
            db.DeclareWin(username);
            Assert.Equal(db.GetPlayerState(username), "win");
            db.CalcScore(username);
            Assert.Equal(db.GetPlayerState(username), "lobby");
            Assert.True(db.AllStates("lobby"));

            Assert.True(db.Leave(username));
            Assert.False(db.IsConnected(username));
        }
    }
}
