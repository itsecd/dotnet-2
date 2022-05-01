using System;
using System.Threading.Tasks;
using System.Timers;

using Gomoku;

namespace GomokuServer
{
    public sealed class GamingSession
    {
        private static readonly TimeSpan _timeout = TimeSpan.FromSeconds(5);

        public Player FirstPlayer { get; }

        public Player SecondPlayer { get; }

        private readonly Timer _timer = new(_timeout.TotalMilliseconds) { AutoReset = false };

        public GamingSession(Player firstPlayer, Player secondPlayer)
        {
            FirstPlayer = firstPlayer;
            FirstPlayer.Session = this;

            SecondPlayer = secondPlayer;
            SecondPlayer.Session = this;

            _timer.Elapsed += OnTimeout;
        }

        public void Start()
        {
            Task.Run(() =>
            {
                SendFindOpponentReply(FirstPlayer, SecondPlayer.Login);
                SendActivePlayerReply(FirstPlayer, true);
                Console.WriteLine("FirstPlayer");
            });

            Task.Run(() =>
            {
                SendFindOpponentReply(SecondPlayer, FirstPlayer.Login);
                SendActivePlayerReply(SecondPlayer, false);
                Console.WriteLine("SecondPlayer");
            });

            _timer.Start();
        }

        public void MakeTurn(Player player, MakeTurnRequest makeTurnRequest)
        {

        }

        private void OnTimeout(object sender, ElapsedEventArgs e)
        {
            lock (_timer)
            {
                Console.WriteLine(_timer.Enabled);
            }
        }

        private static void SendFindOpponentReply(Player player, string login)
        {
            var findOpponentReply = new FindOpponentReply { Login = login };
            var reply = new Reply { FindOpponentReply = findOpponentReply };
            player.WriteAsync(reply);
        }

        private static void SendActivePlayerReply(Player player, bool yourTurn)
        {
            var activePlayerReply = new ActivePlayerReply { YourTurn = yourTurn };
            var reply = new Reply { ActivePlayerReply = activePlayerReply };
            player.WriteAsync(reply);
        }
    }
}
