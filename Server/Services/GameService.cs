using Grpc.Core;
using Microsoft.Extensions.Logging;
using SnakeServer;
using SnakeServer.Database;
using System.Threading.Tasks;

namespace Server
{
    public class GameService 
    {
        private readonly ILogger<GameService> _logger;
        private readonly IGameRepository _players;
        private readonly IGameNetwork _users;

        public GameService(ILogger<GameService> logger, IGameRepository players, IGameNetwork users)
        {
            _logger = logger;
            _players = players;
            _users = users;
            _players.ReadFile();
        }
        public async Task Play(IAsyncStreamReader<PlayerMessage> requestStream, IServerStreamWriter<ServerMessage> responseStream)
        {
            if (!await requestStream.MoveNext()) return; // посмотреть видео 
            var initMessage = requestStream.Current;
            string playerName = initMessage.LoginRequest.Name;
            _logger.LogTrace("[{username}] присоединился к комнате", playerName);
            //добавление пользователя, если он заходит первый раз 
            if (_players.TryAddPlayer(playerName))
                await _players.WriteFile();
            // Присоединение пользователя 
            _users.Join(playerName,responseStream);
            do
            {

            } while (_users.IsConnected(playerName));
        } 
    }
}
