using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GomokuClient
{
    class Player
    {
        public string Login { get; }
        public bool IsFirstTurn { get; }

        public Player(string login, bool isFirstTurn)
        {
            Login = login;
            IsFirstTurn = isFirstTurn;
        }
    }
}
