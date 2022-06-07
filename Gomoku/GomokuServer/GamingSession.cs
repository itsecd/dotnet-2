using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

using Gomoku;

namespace GomokuServer
{
    public sealed class GamingSession
    {
        private readonly Playground _playground = new();

        private readonly Player _firstPlayer;

        private readonly Player _secondPlayer;

        private bool _isFirstTurn;

        public GamingSession(Player firstPlayer, Player secondPlayer)
        {
            _firstPlayer = firstPlayer;
            _firstPlayer.Session = this;

            _secondPlayer = secondPlayer;
            _secondPlayer.Session = this;
        }

        public void Start()
        {
            _isFirstTurn = true;

            Task.Run(() =>
            {
                SendFindOpponentReply(_firstPlayer, _secondPlayer.Login, true);
            });

            Task.Run(() =>
            {
                SendFindOpponentReply(_secondPlayer, _firstPlayer.Login, false);
            });
        }

        public void MakeTurn(Player player, MakeTurnRequest makeTurnRequest)
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
            gameplay.EnterIntoTheCell(point, _isFirstTurn);

            SendMakeTurnReply(notActivePlayer, point, true);
            SendMakeTurnReply(activePlayer, point, false);

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
                _isFirstTurn = !_isFirstTurn;
            }
        }

        private static void SendFindOpponentReply(Player player, string login, bool yourTurn)
        {
            var findOpponentReply = new FindOpponentReply { Login = login, YourTurn = yourTurn };
            var reply = new Reply { FindOpponentReply = findOpponentReply };
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
