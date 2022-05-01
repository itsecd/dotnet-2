using System;
using System.Threading;
using System.Threading.Tasks;

using Gomoku;

using Grpc.Net.Client;

using static Gomoku.Gomoku;

namespace GomokuConsoleClient
{
    public static class Program
    {
        public static async Task Main()
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:5000");
            {
                await using var session = new SessionWrapper(channel);

                await session.FirstPlayer.Login("Lera");
                await session.SecondPlayer.Login("Danila");

                await session.FirstPlayer.FindOpponent();
                await session.SecondPlayer.FindOpponent();

                await Task.Delay(1000);
            }



            Console.ReadKey();
        }
    }
}
