using Snake;
using System.Threading.Tasks;



namespace SnakeServer
{
    public sealed class GamingSession
    {
        private readonly Player _firstPlayer;

        private readonly Player _secondPlayer;
        public GamingSession()
        {
            _firstPlayer = new Player();
            _secondPlayer = new Player();
        }
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
