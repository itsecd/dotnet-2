using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SudokuServer.Models;

namespace SudokuServer.Repositories
{
    public interface IPlayersRepository
    {
        public Task<bool> AddPlayer(Player player);
        public Task<Player?> GetPlayer(string login);
        public Task RemovePlayer(string login);
    }
}
