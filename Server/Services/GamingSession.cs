using SnakeServer;
using SnakeServer.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace Server.Services
{
    public class GamingSession
    {
        private static readonly TimeSpan _timeout = TimeSpan.FromSeconds(1000);

        public Player _firstPlayer { get; }

        public Player _secondPlayer { get; }

        private readonly Timer _timer = new(_timeout.TotalMilliseconds) { AutoReset = false };

        private bool _isTimerActive;

        public GamingSession(Player firstPlayer, Player secondPlayer)
        {
            _firstPlayer = firstPlayer;
            _firstPlayer.Session = this;

            _secondPlayer = secondPlayer;
            _secondPlayer.Session = this;

        }
        public void Start()
        {

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

        public void SendResultForFirstPlayer(Player player,String numberOfPoints)
        {
           
        }
        private static void SendFindOpponentReply(Player player, string login)
        {
            var findOpponentReply = new FindOpponentReply { Login = login };
            var reply = new ServerMessage { FindOpponentReply = findOpponentReply };
            player.WriteAsync(reply);
        }

        private static void SendActivePlayerReply(Player player, bool yourTurn)
        {
            var activePlayerReply = new ActivePlayerReply { YourTurn = yourTurn };
            var reply = new ServerMessage { ActivePlayerReply = activePlayerReply };
            player.WriteAsync(reply);
        }

        
    }
}
