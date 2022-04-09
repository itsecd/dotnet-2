using System.Threading.Tasks;

namespace MinesweeperServer.Database
{
    public interface IGameRepository
    {
        Player this[string name] { get; }
        bool TryAddPlayer(string name);
        void Load();
        Task DumpAsync();
        bool CalcScore(string name, string state);
    }
}