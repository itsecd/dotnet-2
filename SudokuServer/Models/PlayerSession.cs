using System.Threading.Tasks;

using Grpc.Core;


namespace SudokuServer.Models
{
    public class PlayerSession
    {
        public Player Player { get; }
        public string Login { get { return Player.Login; } }

        private readonly IAsyncStreamReader<Request> _requestStream;
        private readonly IServerStreamWriter<Event> _responseStream;
        private readonly GameSession _gameSession;

        public PlayerSession(Player player, IAsyncStreamReader<Request> requestStream, IServerStreamWriter<Event> responseStream)
        {
            Player = player;
            _requestStream = requestStream;
            _responseStream = responseStream;
            _gameSession = new GameSession(player, requestStream, responseStream);
        }

        public Task StartGame()
        {
            return _gameSession.Start();
        }
    }
}
