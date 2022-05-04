using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Gomoku;

using Grpc.Core;
using Grpc.Net.Client;

using static Gomoku.Gomoku;

namespace GomokuConsoleClient
{
    sealed class PlayerWrapper : IAsyncDisposable
    {
        
        public List<Reply> Replies { get; } = new();

        private readonly AsyncDuplexStreamingCall<Request, Reply> _stream;
        private readonly Task _responseTask;

        public PlayerWrapper(GrpcChannel channel)
        {
            var client = new GomokuClient(channel);
            _stream = client.Play();

            _responseTask = Task.Run(async () =>
            {
                while (await _stream.ResponseStream.MoveNext(CancellationToken.None))
                {
                    var reply = _stream.ResponseStream.Current;

                    Replies.Add(reply);
                }
            });
        }

        public async Task Login(string login)
        {
            var loginRequest = new LoginRequest { Login = login };
            var request = new Request { LoginRequest = loginRequest };
            await _stream.RequestStream.WriteAsync(request);
        }

        public async Task FindOpponent()
        {
            var findOpponentRequest = new FindOpponentRequest();
            var request = new Request { FindOpponentRequest = findOpponentRequest };
            await _stream.RequestStream.WriteAsync(request);
        }

        public async Task MakeTurn(Point point)
        {
            var makeTurnRequest = new MakeTurnRequest { Point = point };
            var request = new Request { MakeTurnRequest = makeTurnRequest };
            await _stream.RequestStream.WriteAsync(request);
        }

        public async ValueTask DisposeAsync()
        {
            await _stream.RequestStream.CompleteAsync();
            await _responseTask;
            _stream.Dispose();
        }
    }
}
