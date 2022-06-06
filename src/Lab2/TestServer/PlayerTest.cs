using Xunit;
using Lab2.Model;
using Lab2.Service;

namespace TestServer
{
    public class PlayerTest
    {
        private static Player Creat(string id, string name, string position)
        {
            var player = new Player(id, name, position);
            return player;
        }

        [Fact]
        public void PlayerSerializerTest()
        {
            // arrange
            var testPlayer = Creat("1", "Harry", "Left");
            PlayerSerializer serializer = new(); 
            // act
            serializer.SerializerPlayer(testPlayer);
            var listPlayer = serializer.DeserializerPlayer()[0];
            //assert
            Assert.Equal(testPlayer, listPlayer);
        }
    }
}
