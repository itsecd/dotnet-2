using System.Threading.Tasks;

using Grpc.Core;


namespace SudokuServer.Models
{
    public class GameSession
    {
        private readonly Player _player;
        private readonly IAsyncStreamReader<Request> _requestStream;
        private readonly IServerStreamWriter<Event> _responseStream;

        public GameSession(Player player, IAsyncStreamReader<Request> requestStream, IServerStreamWriter<Event> responseStream)
        {
            _player = player;
            _requestStream = requestStream;
            _responseStream = responseStream;
        }

        public async Task Start()
        {
            while (await _requestStream.MoveNext())
            {
                if (_requestStream.Current.RequestCase == Request.RequestOneofCase.Play)
                {
                    _player.Score += 1;
                    var playEvent = new PlayEvent();
                    await _responseStream.WriteAsync(new Event() { Play = playEvent });
                }
            }

        }

    }
}
