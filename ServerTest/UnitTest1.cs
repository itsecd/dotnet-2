using System;
using Xunit;
using MSO_Server;

namespace ServerTest
{
    public class DatabaseTest
    {
        [Fact]
        public void Test1()
        {
            GameDatabase db = new();
            Assert.True(db.TryAdd("DimaDivan"));
        }
    }
}
