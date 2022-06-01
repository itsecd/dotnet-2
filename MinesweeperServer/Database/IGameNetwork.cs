using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;

namespace MinesweeperServer.Database
{
    public interface IGameNetwork
    {
        public User this[string name] {get;}
        bool Join(string name, IServerStreamWriter<GameMessage> channel);
        bool Leave(string name);
        bool IsConnected(string name);
        bool AllStates(string name);
        void SetPlayerState(string name, string state);
        string GetPlayerState(string name);
        Task SendPlayer(string name, string player_name, Player player_stats);
        Task Broadcast(GameMessage message, string name = "");
        public ICollection<string> GetPlayers {get;}
    }
}