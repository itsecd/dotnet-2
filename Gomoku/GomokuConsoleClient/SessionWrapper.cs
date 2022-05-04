using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grpc.Net.Client;

namespace GomokuConsoleClient
{
    sealed class SessionWrapper: IAsyncDisposable
    {
        public PlayerWrapper FirstPlayer { get;}
        public PlayerWrapper SecondPlayer { get; }

        public SessionWrapper(GrpcChannel channel)
        {
            FirstPlayer = new PlayerWrapper(channel);
            SecondPlayer = new PlayerWrapper(channel);
        }

        public async ValueTask DisposeAsync()
        {
            await FirstPlayer.DisposeAsync();
            await SecondPlayer.DisposeAsync();
        }
    }
}
