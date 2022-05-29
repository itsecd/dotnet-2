

using Grpc.Core;
using Grpc.Net.Client;
using SnakeServer;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeClient
{
    sealed class PlayerWrapper : IAsyncDisposable
    {
        public List<ServerMessage> Replies { get; } = new();

        private readonly AsyncDuplexStreamingCall<PlayerMessage, ServerMessage> _stream;
        private readonly Task _responseTask;

        public PlayerWrapper(GrpcChannel channel)
        {
            var client = new Snake.SnakeClient(channel);
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
            var loginRequest = new LoginRequest { Name = login };
            var request = new PlayerMessage { LoginRequest = loginRequest };
            await _stream.RequestStream.WriteAsync(request);
        }

        public async Task FindOpponent()
        {
            var findOpponentRequest = new FindOpponentRequest();
            var request = new PlayerMessage { FindOpponentRequest = findOpponentRequest };
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

