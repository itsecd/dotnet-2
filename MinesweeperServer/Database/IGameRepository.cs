using System.Threading.Tasks;

namespace MinesweeperServer.Database
{
    public interface IGameRepository
    {
        Player this[string name] { get; }
        bool TryAdd(string name);
        void Load();
        void Dump();
        Task LoadAsync();
        Task DumpAsync();
        bool CalcScore(string name, string state);
    }
}