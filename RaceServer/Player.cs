using Grpc.Core;
using System;
using System.Threading.Tasks;

namespace RaceServer
{
    public sealed class Player
    {
        public string Login { get; set; }
        public int CountGames { get; set; }
        public int CountWinGames { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public GamingSession? Session { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        private readonly IServerStreamWriter<Event>? _responseStream;
        [System.Text.Json.Serialization.JsonIgnore]
        private Task _responseStreamTask = Task.CompletedTask;
        private IServerStreamWriter<Event> ResponseStream => _responseStream ?? throw new InvalidOperationException();

        public Player()
        {
            Login = "";
            _responseStream = null;
        }

        public Player(string login, IServerStreamWriter<Event> responseStream)
        {
            Login = login;
            _responseStream = responseStream;
        }
        public Player(string login, int countGames, int countWinGames, IServerStreamWriter<Event> responseStream)
        {
            Login = login;
            CountGames = countGames;
            CountWinGames = countWinGames;
            _responseStream = responseStream;
        }

        public void WriteAsync(Event reply)
        {
            lock (ResponseStream)
            {
                if (_responseStreamTask.IsCompleted)
                {
                    _responseStreamTask = ResponseStream.WriteAsync(reply);
                    return;
                }

                _responseStreamTask = _responseStreamTask.ContinueWith(t =>
                {
                    ResponseStream.WriteAsync(reply);
                });
            }
        }

        public override string ToString()
        {
            return "\nLogin " + Login + "\nCount: " + CountGames + "\nWin: " + CountWinGames + "\n";
        }
    }
}
