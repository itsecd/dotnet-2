using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

using Gomoku;

namespace GomokuServer
{
    public sealed class GamingSession
    {
        private static readonly TimeSpan _timeout = TimeSpan.FromSeconds(1000);

        public Player FirstPlayer { get; }

        public Player SecondPlayer { get; }

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
                var gameplay = new Gameplay();

                var activePlayer = FirstPlayer;
                var notActivePlayer = SecondPlayer;

                if (!_isFirstTurn)
                {
                    activePlayer = SecondPlayer;
                    notActivePlayer = FirstPlayer;
                }

                if (player != activePlayer)
                    throw new ApplicationException("Not your turn");

                var point = makeTurnRequest.Point;

                gameplay.CellIsBusy(point);

                _timer.Stop();
                _isTimerActive = false;

                gameplay.EnterIntoTheCell(point, _isFirstTurn);

                SendMakeTurnReply(notActivePlayer, point, _isFirstTurn);

                SendMakeTurnReply(activePlayer, point, !_isFirstTurn);

                var gameOver = gameplay.CheckGame();

                if (gameOver)
                {

                    List<Point> WinPoints = gameplay._winPoints;

                    if (gameplay._winner == Gameplay.Cell.Empty)
                    {
                        var status = OutcomeStatus.Defeat;
                        SendEndGameReply(FirstPlayer, status, WinPoints);
                        SendEndGameReply(SecondPlayer, status, WinPoints);
                    }
                    else
                    {
                        SendEndGameReply(activePlayer, OutcomeStatus.Victory, WinPoints);
                        SendEndGameReply(notActivePlayer, OutcomeStatus.Draw, WinPoints);
                    }
                }

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

        private static void SendEndGameReply(Player player, OutcomeStatus status, List<Point> WinPoints)
        {
            var endGameReply = new EndGameReply { Status = status };
            endGameReply.Points.Add(WinPoints.ToArray());
            var reply = new Reply { EndGameReply = endGameReply };
            player.WriteAsync(reply);
        }
    }
}
