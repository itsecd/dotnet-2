using System.Threading.Tasks;

using Gomoku;

using Grpc.Core;

namespace GomokuServer
{
    public sealed class Player
    {
        public string Login { get; }

        public GamingSession? Session { get; set; }

        private readonly IServerStreamWriter<Reply> _responseStream;

        private Task _responseStreamTask = Task.CompletedTask;

        public Player(string login, IServerStreamWriter<Reply> responseStream)
        {
            Login = login;
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
    }
}
