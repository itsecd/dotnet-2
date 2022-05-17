using System;
using Xunit;
using Lab2;
using Lab2.Model;
using Lab2.Repositories;

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
        public void AddPlayer()
        {
            // arrange
            var testPlayer = Creat("1", "Harry", "Left");
            PlayerRepository repository = new();
            // act
            repository.Add(testPlayer);
            // assert
            Assert.Equal(testPlayer.ConnectionId, repository.GetPlayer("Harry").ConnectionId);
            Assert.Equal(testPlayer.PlayerPosition, repository.GetPlayer("Harry").PlayerPosition);
            repository.Remove(testPlayer.UserName);
        }

        [Fact]
        public void Delete()
        {
            // arrange
            var testPlayer = Creat("1", "Harry", "Left");
            PlayerRepository repository = new();
            repository.Add(testPlayer);
            // act
            string name = repository.Remove("Harry");
            // assert
            Assert.Equal(testPlayer.UserName, name);
        }

        [Fact]
        public void Clear()
        {
            // arrange
            var testPlayer = Creat("1", "Harry", "Left");
            PlayerRepository repository = new();
            repository.Add(testPlayer);
            // act
            repository.Clear();
            //assert
            Assert.True(!repository.ListPlayers().Contains(testPlayer));
        }

        [Fact]
        public void GetPlayer()
        {
            //arrange
            var testPlayer = Creat("1", "Harry", "Left");
            PlayerRepository repository = new();
            repository.Add(testPlayer);
            //act
            Player player = repository.GetPlayer("Harry");
            //assert
            Assert.Equal(testPlayer, player);
        }
    }
}
