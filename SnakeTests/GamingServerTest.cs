using Grpc.Core;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using Snake;
using SnakeServer;
using SnakeServer.Services;
using System;
using System.Collections.Generic;
using System.IO;
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
        public void WriteAsyncTest()
        {
            var gamingServer = new GamingServer();
            gamingServer.WriteToFile();
            Assert.True(File.Exists("players.xml"));
        }

        [Fact]
        public void SendResultGameTest()
        {
            var gamingServer = new GamingServer();
            Player player = new Player();
            player.Login = "valera";
            SendResultGame resultGame = new SendResultGame();
            resultGame.Score = 10;
            gamingServer.SendResult(player, resultGame);
            var mock = new Mock<IServerStreamWriter<Reply>>();
            player.Score = resultGame.Score;
            Assert.True(gamingServer.GetPlayerFromFile(player.Login, mock.Object).Result.Login.Equals(player.Login));
        }



    }
}
