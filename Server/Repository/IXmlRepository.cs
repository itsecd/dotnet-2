using Grpc.Core;
using SnakeServer;
using SnakeServer.Database;
using System;
using System.Threading.Tasks;

namespace Server.Repository
{
    public interface IXmlRepository
    {
        Task<Player> GetPlayerFromFile(String login, IServerStreamWriter<ServerMessage> responseStream);
        void SavePlayerToFile(Player player);
        bool CompareResult();
        Task ReadFileWithPlayersAsync();

    }
}
