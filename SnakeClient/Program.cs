using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SnakeClient
{
    public class Program
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
                    await session.FirstPlayer.Login("valera");
                    await session.SecondPlayer.Login("Lera");
/*
                    await session.FirstPlayer.FindOpponent();
                    await Task.Delay(1000);
                    await session.SecondPlayer.FindOpponent();

                    await Task.Delay(1000);
*/
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


