using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Net.Client;

namespace MinesweeperClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Write("Nickname: ");
            string nickname = Console.ReadLine();
            var channel = GrpcChannel.ForAddress("http://localhost:5000");
            var client = new Minesweeper.MinesweeperClient(channel);
            using (var connection = client.Join())
            {
                _ = Task.Run(async () =>
                {
                    while (await connection.ResponseStream.MoveNext(cancellationToken: CancellationToken.None))
                    {
                        var response = connection.ResponseStream.Current;
                        Console.Write($"text: {response.Text}\nstate: {response.State}\n");
                    }
                });
                string text = "";
                await connection.RequestStream.WriteAsync(new PlayerMessage{Name = nickname});
                while (text != "leave")
                {
                    Console.Write(">>> ");
                    text = Console.ReadLine();
                    await connection.RequestStream.WriteAsync(new PlayerMessage{Name = nickname, Text = text});
                }
                await connection.RequestStream.CompleteAsync();
            }
            Console.WriteLine("disconnecting...");
            await channel.ShutdownAsync();
        }
    }
}
