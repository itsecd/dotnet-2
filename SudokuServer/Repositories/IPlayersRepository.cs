using System.Threading.Tasks;

using SudokuServer.Models;

namespace SudokuServer.Repositories
{
    public interface IPlayersRepository
    {
        public Task<bool> AddPlayer(Player player);
        public Task<Player?> GetPlayer(string login);
        public Task<bool> UpdatePlayer(Player player);
        public Task RemovePlayer(string login);
    }
}
