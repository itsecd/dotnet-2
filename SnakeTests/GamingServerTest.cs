using Grpc.Core;
using Moq;
using Server;
using Server.Services;
using Snake;
using System.IO;
using Xunit;

namespace SnakeTests
{
    public class GamingServerTest
    {

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
