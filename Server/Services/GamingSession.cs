using SnakeServer;
using SnakeServer.Database;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace Server.Services
{
    public class GamingSession
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

            //_timer.Elapsed += OnTimeout;
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
