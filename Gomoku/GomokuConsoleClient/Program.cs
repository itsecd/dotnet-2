using System;
using System.Threading.Tasks;

using Gomoku;

using Grpc.Net.Client;

namespace GomokuConsoleClient
{
    public static class Program
    {
        public static async Task Main()
        {
            SessionWrapper sessionWrapper;

            using var channel = GrpcChannel.ForAddress("http://localhost:5000");
            {
                await using var session = new SessionWrapper(channel);
                sessionWrapper = session;

                try
                {
                    await session.FirstPlayer.Login("Anton");
                    await session.SecondPlayer.Login("Danila");

                    await session.FirstPlayer.FindOpponent();
                    await Task.Delay(1000);
                    await session.SecondPlayer.FindOpponent();

                    await Task.Delay(1000);

                    await session.FirstPlayer.MakeTurn(new Point { X = 0, Y = 0 });
                    await Task.Delay(1000);
                    await session.SecondPlayer.MakeTurn(new Point { X = 1, Y = 0 });

                    await Task.Delay(1000);

                    await session.FirstPlayer.MakeTurn(new Point { X = 0, Y = 1 });
                    await Task.Delay(1000);
                    await session.SecondPlayer.MakeTurn(new Point { X = 1, Y = 2 });

                    await Task.Delay(1000);

                    await session.FirstPlayer.MakeTurn(new Point { X = 0, Y = 2 });
                    await Task.Delay(1000);
                    await session.SecondPlayer.MakeTurn(new Point { X = 7, Y = 2 });

                    await Task.Delay(1000);

                    await session.FirstPlayer.MakeTurn(new Point { X = 0, Y = 3 });
                    await Task.Delay(1000);
                    await session.SecondPlayer.MakeTurn(new Point { X = 2, Y = 8 });

                    await Task.Delay(1000);

                    await session.FirstPlayer.MakeTurn(new Point { X = 0, Y = 4 });
                    await Task.Delay(1000);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            foreach (var reply in sessionWrapper.FirstPlayer.Replies)
                Console.WriteLine(reply);

            Console.WriteLine();

            foreach (var reply in sessionWrapper.SecondPlayer.Replies)
                Console.WriteLine(reply);

            Console.ReadKey();
        }
    }
}
