using Lab2.Model;
using System.Collections.Generic;

namespace Lab2.Repositories
{
    public interface IPlayerRepository
    {
        string Add(Player player);

        void Clear();

        string Remove(string name);

        List<Player> ListPlayers();

        Player GetPlayer(string name);
    }
}