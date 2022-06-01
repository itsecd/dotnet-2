using Grpc.Core;

namespace MinesweeperServer.Database
{
    /// <summary>Класс для хранения информации о пользователе.</summary>
    public class User
    {
        public IServerStreamWriter<GameMessage> Channel;
        public string State;
    }
}