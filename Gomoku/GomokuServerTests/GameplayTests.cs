using System.Collections.Generic;

using Gomoku;

using Xunit;

namespace GomokuServer.Tests
{
    public class GameplayTests
    {
        [Fact]
        public void EnterIntoTheCellTest()
        {
            var gameField = new FieldExtensions();
            var gameplay = new Gameplay(gameField);
            Point point = new() { X = 1, Y = 5 };
            gameplay.EnterIntoTheCell(point, true);
            FieldExtensions.Cell expectedCell = FieldExtensions.Cell.FirstPlayer;
            var expectedPoint = new Point { X = 1, Y = 5 };

            var actualCell = gameplay._gameField[1, 5];
            var actualPoint = point;

            Assert.Equal(expectedCell, actualCell);
            Assert.Equal(expectedPoint, actualPoint);
        }

        [Fact]
        public void CheckFieldTest()
        {
            var gameField = new FieldExtensions();
            var gameplay = new Gameplay(gameField);
            var expected = new List<Point>();
            for (var i = 0; i < 5; ++i)
            {
                expected.Add(new Point { X = 1, Y = i + 1 });
                gameplay.EnterIntoTheCell(expected[i], true);
            }
            gameplay._point = expected[0];

            var actual = gameplay.CheckField(FieldExtensions.Cell.FirstPlayer, 0, 1);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckDefeatTest()
        {
            var gameField = new FieldExtensions();
            var gameplay = new Gameplay(gameField);
            gameplay.CheckDraw();
            FieldExtensions.Cell? expected = null;

            var actual = gameplay._winner;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckGameTest()
        {
            var gameField = new FieldExtensions();
            var gameplay = new Gameplay(gameField);
            Point point = new() { X = 1, Y = 5 };
            gameplay.EnterIntoTheCell(point, true);
            var expected = false;

            var actual = gameplay.CheckGame();

            Assert.Equal(expected, actual);
        }
    }
}
