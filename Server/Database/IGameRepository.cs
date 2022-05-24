using System.Threading.Tasks;

namespace SnakeServer.Database
{
    public interface IGameRepository
    {
        Player this[string name] { get; }
        bool TryAddPlayer(string name);
        void ReadFile();
        Task WriteFile();
    }
}