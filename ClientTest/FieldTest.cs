using Xunit;
using MinesweeperClient.Models;
namespace ClientTest;

public class FieldTest
{
    [Fact]
    public void ConstructorTest()
    {
        MinesweeperField Field = new(100, 100);
        Assert.Equal(GameStates.Ready, Field.GameState());
    }
}