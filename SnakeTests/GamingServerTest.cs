using SnakeServer.Services;
using System;
using Xunit;
using Xunit.Abstractions;

namespace SnakeTests
{
    public class GamingServerTest
    {

        protected readonly ITestOutputHelper Output;

        public GamingServerTest(ITestOutputHelper tempOutput)
        {
            Output = tempOutput;
        }

        [Fact]
        public async void WriteAsyncTest()
        {

        }
    }
}
