using Grpc.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Server.Services;
using SnakeServer;
using SnakeServer.Database;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Server
{
    public sealed class GameService 
    {
        private readonly ConcurrentDictionary<string, Player> _players = new();
        private readonly object _waitingPlayerLock = new();
        private Player? _waitingPlayer;
/*        private readonly string _filePath;
        public GameService(IConfiguration config)
        {
            _filePath = config.GetValue<string>("PathPlayers");
        }*/
        public async Task Join(IAsyncStreamReader<PlayerMessage> requestStream, IServerStreamWriter<ServerMessage> responseStream)
        {

            if (!await requestStream.MoveNext())
                return;
            var player = Login(requestStream.Current.LoginRequest, responseStream);

            try
            {
                var loginReply = new LoginReply { Success = player is not null };
                await responseStream.WriteAsync(new ServerMessage { LoginReply = loginReply });
                if (player is null)
                    return;

                while (await requestStream.MoveNext())
                {
                    switch (requestStream.Current.PlayerMessageCase)
                    {
                        case PlayerMessage.PlayerMessageOneofCase.None:
                            throw new ApplicationException();
                        case PlayerMessage.PlayerMessageOneofCase.LoginRequest:
                            throw new ApplicationException();
                        case PlayerMessage.PlayerMessageOneofCase.FindOpponentRequest:
                            FindOpponent(player);
                            break;
                        default: throw new ApplicationException();
                    }

                }
            }
            finally
            {
                if (player is not null)
                    _players.TryRemove(player.Login, out _);
            }

        }
        private Player? Login(LoginRequest loginRequest, IServerStreamWriter<ServerMessage> responseStream)
        {
            var player = new Player(loginRequest.Name, responseStream);
            return _players.TryAdd(player.Login, player) ? player : null;
        }

        private void FindOpponent(Player player)
        {
            GamingSession session;
            lock (_waitingPlayerLock)
            {
                if (_waitingPlayer is null)
                {
                    _waitingPlayer = player;
                    return;
                }

                session = new GamingSession(_waitingPlayer, player);
                _waitingPlayer = null;
            }
            session.Start();
        }

        public static implicit operator GameService(GamingSession v)
        {
            throw new NotImplementedException();
        }
    }

}
