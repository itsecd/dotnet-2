using Grpc.Core;
using Snake;
using SnakeServer;
using System;
using System.Threading.Tasks;

namespace Server.Repository
{
    public interface IXmlRepository
    {
        void ReadFromFile();
        Task<Player> GetPlayerFromFile(String login, IServerStreamWriter<Reply> responseStream);
        void WriteToFile();
        void SavePlayerToFile(Player player);
    }
}
