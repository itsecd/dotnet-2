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

        public enum PlayersCage
        {
            Empty = 0,
            FirstPlayer,
            SecondPlayer
        }

        private PlayersCage[,] _field = new PlayersCage[15, 15];

        public Player SecondPlayer { get; }

        private readonly Timer _timer = new(_timeout.TotalMilliseconds) { AutoReset = false };

        public GamingSession(Player firstPlayer, Player secondPlayer)
        {
            FirstPlayer = firstPlayer;
            FirstPlayer.Session = this;

            SecondPlayer = secondPlayer;
            SecondPlayer.Session = this;

            for (var i = 0; i < 15; ++i)
                for (var j = 0; j < 15; ++j)
                    _field[i, j] = PlayersCage.Empty;

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
            var point = makeTurnRequest.Point;

            bool firstPlayer = false;

            if (player == FirstPlayer)
                firstPlayer = true;

            if (_field[point.X, point.Y] == PlayersCage.Empty)
                if (firstPlayer)
                    _field[point.X, point.Y] = PlayersCage.FirstPlayer;
                else
                    _field[point.X, point.Y] = PlayersCage.SecondPlayer;

            if (firstPlayer)
                SendMakeTurnReply(SecondPlayer, point);
            else
                SendMakeTurnReply(FirstPlayer, point);

            //for (var i = 0; i < 15; ++i)
            //{
            //    Console.WriteLine("\n");
            //    for (var j = 0; j < 15; ++j)
            //        Console.Write($"{_field[i, j]} ");
            //}
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

        private static void SendMakeTurnReply(Player player, Point point)
        {
            var makeTurnReply = new MakeTurnReply { Point = point, YourTurn = true };
            var reply = new Reply { MakeTurnReply = makeTurnReply };
            player.WriteAsync(reply);
        }
    }
}
