using Lab2.Model;
using System;
using System.Collections.Generic;

namespace Lab2.Repositories
{
    public interface IPlayerRepository
    {
        void Add(Player player);

        void Clear();

        void RemoveAt(int indeX);

        List<Player> GetPlayers();
    }
}