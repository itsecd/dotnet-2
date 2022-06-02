using System.Threading.Tasks;

namespace RaceServer
{
    public sealed class GamingSession
    {

        private readonly Player _firstPlayer;

        private readonly Player _secondPlayer;


        public GamingSession(Player firstPlayer, Player secondPlayer)
        {
            _firstPlayer = firstPlayer;
            _firstPlayer.Session = this;

            _secondPlayer = secondPlayer;
            _secondPlayer.Session = this;

            _firstPlayer.CountGames++;
            _secondPlayer.CountGames++;
        }

        public void Start()
        {

            Task.Run(() =>
            {
                SendFindOpponentEvent(_firstPlayer, _secondPlayer.Login);
            });

            Task.Run(() =>
            {
                SendFindOpponentEvent(_secondPlayer, _firstPlayer.Login);
            });

        }

        public void ChangePositionEvent(Player player, ChangePositionRequest changePositionRequest)
        {
            if (player == _firstPlayer)
                SendOpponentPositionEvent(_secondPlayer, changePositionRequest.Position, changePositionRequest.Rotate);
            else
                SendOpponentPositionEvent(_firstPlayer, changePositionRequest.Position, changePositionRequest.Rotate);
        }

        public void ResultEvent(Player player, WinGameRequest winGameRequest)
        {
            if (player == _firstPlayer)
            {
                SendResult(_firstPlayer, true);
                SendResult(_secondPlayer, false);
            }
            else
            {
                SendResult(_secondPlayer, true);
                SendResult(_firstPlayer, false);
            }
        }

        private static void SendFindOpponentEvent(Player player, string login)
        {
            var findOpponentEvent = new FindOpponentEvent { OpponentLogin = login };
            var ev = new Event { FindOpponent = findOpponentEvent };
            player.WriteAsync(ev);
        }

        private static void SendOpponentPositionEvent(Player player, Point position, float rotate)
        {
            var opponentPositionEvent = new OpponentPositionEvent { Position = position, Rotate = rotate };
            var ev = new Event { OpponentPosition = opponentPositionEvent };
            player.WriteAsync(ev);
        }

        private static void SendResult(Player player, bool result)
        {
            var resultGameEvent = new ResultGameEvent { Win = result };
            var ev = new Event { ResultGame = resultGameEvent };
            player.WriteAsync(ev);
        }
    }
}