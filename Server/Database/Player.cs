using Grpc.Core;
using Server;
using System.Threading.Tasks;

namespace SnakeServer.Database
{
    /// <summary>Класс для хранения информации об игроке.</summary>
    [System.Serializable]
    public class Player
    {
        public string Login { get; }

        public GameService? Session { get; set; }

        private readonly IServerStreamWriter<ServerMessage> _responseStream;

        private Task _responseStreamTask = Task.CompletedTask;

        public Player(string login, IServerStreamWriter<ServerMessage> responseStream)
        {
            Login = login;
            _responseStream = responseStream;
        }

        public void WriteAsync(ServerMessage reply)
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