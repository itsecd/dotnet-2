using System.Threading.Tasks;
using Grpc.Core;

namespace MinesweeperServer.Database
{
    public interface IGameNetwork
    {
        bool Join(string name, IServerStreamWriter<ServerMessage> channel);
        bool Leave(string name);
        bool IsConnected(string name);
        bool AllStates(string name);
        void SetPlayerState(string name, string state);
        string GetPlayerState(string name);
        Task SendPlayers(string name);
        Task Broadcast(ServerMessage message, string name = "");
    }
}