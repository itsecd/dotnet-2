using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

using Gomoku;

using Grpc.Core;

namespace GomokuServer.Services
{
    public sealed class GamingServer
    {
        private readonly ConcurrentDictionary<string, Player> _players = new();
        private readonly object _waitingPlayerLock =  new();
        private Player? _waitingPlayer;

        public async Task Play(IAsyncStreamReader<Request> requestStream, IServerStreamWriter<Reply> responseStream)
        {
            if (!await requestStream.MoveNext())
                return;

            if (requestStream.Current.RequestCase != Request.RequestOneofCase.LoginRequest)
                return;

            var player = Login(requestStream.Current.LoginRequest, responseStream);
            try
            {
                var loginReply = new LoginReply { Success = player is not null };
                await responseStream.WriteAsync(new Reply { LoginReply = loginReply });
                if (player is null)
                    return;


                while (await requestStream.MoveNext())
                {
                    switch (requestStream.Current.RequestCase)
                    {
                        case Request.RequestOneofCase.None:
                            throw new ApplicationException();
                        case Request.RequestOneofCase.LoginRequest:
                            throw new ApplicationException();
                        case Request.RequestOneofCase.FindOpponentRequest:
                            FindOpponent(player, requestStream.Current.FindOpponentRequest);
                            break;
                        case Request.RequestOneofCase.MakeTurnRequest:
                            player.Session?.MakeTurn(player, requestStream.Current.MakeTurnRequest);
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

        private Player? Login(LoginRequest loginRequest, IServerStreamWriter<Reply> responseStream) {
            var player = new Player(loginRequest.Login, responseStream);
            return _players.TryAdd(player.Login, player)? player: null;
        }

        private void FindOpponent(Player player, FindOpponentRequest findOpponentRequest)
        {
            GamingSession session;
            lock (_waitingPlayerLock)
            {
                if(_waitingPlayer is null)
                {
                    _waitingPlayer = player;
                    return;
                }

                session = new GamingSession(player, _waitingPlayer);
                _waitingPlayer = null;
            }
            session.Start();
        }
    }
}

