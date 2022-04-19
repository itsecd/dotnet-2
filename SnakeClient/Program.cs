using Grpc.Net.Client;
using Server;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var userName = Console.ReadLine();
            var channel = GrpcChannel.ForAddress("http://localhost:5000");
            var client = new Snake.SnakeClient(channel);

            using(var chat = client.Join())
            {
                _ = Task.Run(async () =>
                {
                    while (await chat.ResponseStream.MoveNext(cancellationToken: CancellationToken.None))
                    {
                        var responce = chat.ResponseStream.Current;
                        Console.WriteLine($"{responce.Name}: {responce.Message}");
                    }
                });
                await chat.RequestStream.WriteAsync(new PlayerMessage() { Name = userName, Message = $"{userName} joined" });
                string line;
                while ((line = Console.ReadLine()) != null)
                {
                    if(line == "bye")
                    {
                        break;
                    }
                    await chat.RequestStream.WriteAsync(new PlayerMessage() { Name = userName, Message = line});
                }
                await chat.RequestStream.CompleteAsync();
             }
            Console.WriteLine("Disconnected");
            await channel.ShutdownAsync();

        }
    }
}
