using System;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Grpc.Core;
using Snake;

namespace SnakeServer
{
    [System.Serializable]
    public sealed class Player
    {
        public string Login { get; set; }
        public int Score { get; set; }
        [XmlIgnore]
        public GamingSession? Session { get; set; }
        
        private readonly IServerStreamWriter<Reply>? _responseStream;
       
        private Task _responseStreamTask = Task.CompletedTask;
        private IServerStreamWriter<Reply> ResponseStream => _responseStream ?? throw new InvalidOperationException();

    
        public Player()
        {
            Login = "";
            _responseStream = null;
        }

        public Player(string login, IServerStreamWriter<Reply> responseStream)
        {
            Login = login;
            _responseStream = responseStream;
        }
        public Player(string login, int countGames, IServerStreamWriter<Reply> responseStream)
        {
            Login = login;
            Score = countGames;
            _responseStream = responseStream;
        }
        public Player(string login, int countGames)
        {
            Login = login;
            Score = countGames;
            _responseStream = null;
        }

        public void WriteAsync(Reply reply)
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
            return "\nLogin " + Login + "\nCount: " + Score + "\n";
        }
    }
}
