using System;
using System.Threading.Tasks;
using System.Timers;

using Gomoku;

namespace GomokuServer
{
    public sealed class GamingSession
    {
        private static readonly TimeSpan _timeout = TimeSpan.FromSeconds(10);

        public Player FirstPlayer { get; }

        public Player SecondPlayer { get; }

        private enum Cell
        {
            Empty = 0,
            FirstPlayer,
            SecondPlayer
        }

        private readonly Cell[,] _field = new Cell[15, 15];

        private readonly Timer _timer = new(_timeout.TotalMilliseconds) { AutoReset = false };

        private bool _isTimerActive;

        private bool _isFirstTurn;

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
            _isFirstTurn = true;

            Task.Run(() =>
            {
                SendFindOpponentReply(FirstPlayer, SecondPlayer.Login);
                SendActivePlayerReply(FirstPlayer, true);
            });

            Task.Run(() =>
            {
                SendFindOpponentReply(SecondPlayer, FirstPlayer.Login);
                SendActivePlayerReply(SecondPlayer, false);
            });

            _isTimerActive = true;
            _timer.Start();
        }

        public void MakeTurn(Player player, MakeTurnRequest makeTurnRequest)
        {
            lock (_timer)
            {

                var activePlayer = _isFirstTurn ? FirstPlayer : SecondPlayer;

                if (player != activePlayer)
                    throw new ApplicationException("Not your turn");

                var point = makeTurnRequest.Point;

                if (_field[point.X, point.Y] != Cell.Empty)
                    throw new ApplicationException("Cell is busy");

                _timer.Stop();
                _isTimerActive = false;


                _field[point.X, point.Y] = _isFirstTurn ? Cell.FirstPlayer : Cell.SecondPlayer;

                SendMakeTurnReply(FirstPlayer, point, _isFirstTurn);

                SendMakeTurnReply(SecondPlayer, point, !_isFirstTurn);

                //game over 

                ChangeActivePlayer();

                _isTimerActive = true;
                _timer.Start();
            }

        }

        private void OnTimeout(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("timer");

            lock (_timer)
            {
                if (!_isTimerActive)
                    return;

                ChangeActivePlayer();
            }
        }

        private void ChangeActivePlayer()
        {
            _isFirstTurn = !_isFirstTurn;
            SendActivePlayerReply(FirstPlayer, _isFirstTurn);
            SendActivePlayerReply(SecondPlayer, !_isFirstTurn);
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

        private static void SendMakeTurnReply(Player player, Point point, bool yourTurn)
        {
            var makeTurnReply = new MakeTurnReply { Point = point, YourTurn = yourTurn };
            var reply = new Reply { MakeTurnReply = makeTurnReply };
            player.WriteAsync(reply);
        }
    }
}
