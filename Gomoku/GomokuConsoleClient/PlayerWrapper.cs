using System;
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
        private readonly AsyncDuplexStreamingCall<Request, Reply> _stream;
        private readonly int _id;
        private readonly Task _responseTask;

        public PlayerWrapper(GrpcChannel channel, int id)
        {
            var client = new GomokuClient(channel);
            _stream = client.Play();
            _id = id;

            _responseTask = Task.Run(async () =>
            {
                while (await _stream.ResponseStream.MoveNext(CancellationToken.None))
                {
                    Console.WriteLine($"{id} {_stream.ResponseStream.Current}");
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
