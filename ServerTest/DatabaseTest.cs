using System;
using Xunit;
using MinesweeperServer;

namespace ServerTest
{
    public class DatabaseTest
    {
        [Fact]
        public void PlayerTest()
        {
            GameDatabase db = new();
            Assert.True(db.TryAdd("DimaDivan"));
        }
    }
}
