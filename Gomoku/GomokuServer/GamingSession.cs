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

        private readonly Playground _playground = new();

        private readonly Player _firstPlayer;

        private readonly Player _secondPlayer;

        private readonly Timer _timer = new(_timeout.TotalMilliseconds) { AutoReset = false };

        private bool _isTimerActive;

        private bool _isFirstTurn;

        public GamingSession(Player firstPlayer, Player secondPlayer)
        {
            _firstPlayer = firstPlayer;
            _firstPlayer.Session = this;

            _secondPlayer = secondPlayer;
            _secondPlayer.Session = this;

            _timer.Elapsed += OnTimeout;
        }

        public void Start()
        {
            _isFirstTurn = true;

            Task.Run(() =>
            {
                SendFindOpponentReply(_firstPlayer, _secondPlayer.Login);
                SendActivePlayerReply(_firstPlayer, true);
            });

            Task.Run(() =>
            {
                SendFindOpponentReply(_secondPlayer, _firstPlayer.Login);
                SendActivePlayerReply(_secondPlayer, false);
            });

            _isTimerActive = true;
            _timer.Start();
        }

        public void MakeTurn(Player player, MakeTurnRequest makeTurnRequest)
        {
            lock (_timer)
            {
                var gameplay = new Gameplay(_playground);

                var activePlayer = _firstPlayer;
                var notActivePlayer = _secondPlayer;

                if (!_isFirstTurn)
                {
                    activePlayer = _secondPlayer;
                    notActivePlayer = _firstPlayer;
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

                    List<Point> WinPoints = gameplay.WinPoints;

                    if (gameplay.Winner == Cell.Empty)
                    {
                        var status = OutcomeStatus.Draw;
                        SendEndGameReply(_firstPlayer, status, WinPoints);
                        SendEndGameReply(_secondPlayer, status, WinPoints);
                    }
                    else
                    {
                        SendEndGameReply(activePlayer, OutcomeStatus.Victory, WinPoints);
                        activePlayer.CountWinGames++;
                        SendEndGameReply(notActivePlayer, OutcomeStatus.Defeat, WinPoints);
                    }
                }
                else
                {
                    ChangeActivePlayer();
                }

                _isTimerActive = true;
                _timer.Start();
            }

        }

        private void OnTimeout(object sender, ElapsedEventArgs e)
        {
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
            SendActivePlayerReply(_firstPlayer, _isFirstTurn);
            SendActivePlayerReply(_secondPlayer, !_isFirstTurn);
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
