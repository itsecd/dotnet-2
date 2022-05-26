using System;
using System.Threading.Tasks;

using Gomoku;

using Grpc.Core;

namespace GomokuServer
{
    public sealed class Player
    {
        public string Login { get; set; }
        public int CountGames { get; set; }
        public int CountWinGames { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public GamingSession? Session { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        private readonly IServerStreamWriter<Reply> _responseStream;
        [System.Text.Json.Serialization.JsonIgnore]
        private Task _responseStreamTask = Task.CompletedTask;

        public Player()
        {
            Login = "";
        }

        public Player(string login, IServerStreamWriter<Reply> responseStream)
        {
            Login = login;
            _responseStream = responseStream;
        }
        public Player(string login, int countGames, int countWinGames, IServerStreamWriter<Reply> responseStream)
        {
            Login = login;
            CountGames = countGames;
            CountWinGames = countWinGames;
            _responseStream = responseStream;
        }

        public void WriteAsync(Reply reply)
        {
            lock (_responseStream)
            {
                if (_responseStreamTask.IsCompleted)
                {
                    _responseStreamTask = _responseStream.WriteAsync(reply);
                    return;
                }

                _responseStreamTask = _responseStreamTask.ContinueWith(t =>
                {
                    _responseStream.WriteAsync(reply);
                });
            }
        }

        public override string ToString()
        {
            return "\nLogin " + Login + "\nCount: " + CountGames + "\nWin: " + CountWinGames + "\n" ;
        }
    }
}
