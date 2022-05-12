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
            var gameplay = new Gameplay();
            Point point = new Point { X = 1, Y = 5 };
            gameplay.EnterIntoTheCell(point, true);
            Gameplay.Cell expectedCell = Gameplay.Cell.FirstPlayer;
            var expectedPoint = new Point { X = 1, Y = 5 };

            var actualCell = gameplay[1, 5];
            var actualPoint = point;

            Assert.Equal(expectedCell, actualCell);
            Assert.Equal(expectedPoint, actualPoint);
        }

        [Fact]
        public void CheckFieldTest()
        {
            var gameplay = new Gameplay();
            var expected = new List<Point>();
            for (var i = 0; i < 5;++i)
            {
                expected.Add(new Point { X = 1, Y = i+1 });
                gameplay.EnterIntoTheCell(expected[i], true);
            }
            gameplay._point = expected[0];

            var actual = gameplay.CheckField(Gameplay.Cell.FirstPlayer, 0, 1);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckDefeatTest()
        {
            var gameplay = new Gameplay();
            gameplay.CheckDefeat();
            Gameplay.Cell? expected = null;

            var actual = gameplay._winner;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckGameTest()
        {
            var gameplay = new Gameplay();
            Point point = new Point { X = 1, Y = 5 };
            gameplay.EnterIntoTheCell(point, true);
            var expected = false;

            var actual = gameplay.CheckGame();

            Assert.Equal(expected, actual);
        }
    }
}
