using ChatServer;
using Grpc.Net.Client;
using System;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var userName = Console.ReadLine();// 

            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new ChatRoom.ChatRoomClient(channel);

            using (var chat = client.Join())
            {
                _ = Task.Run(async () =>
                {
                    while (await chat.ResponseStream.MoveNext(cancellationToken: System.Threading.CancellationToken.None))
                    {
                        var response = chat.ResponseStream.Current;
                        Console.WriteLine($"{response.User}: {response.Text}");
                    }
                });
                await chat.RequestStream.WriteAsync(new Message()

                { User = userName, Text = " has joined the room" });

                string line;
                while ((line = Console.ReadLine()) != null)
                {
                    if (line == "bye")
                    {
                        break;
                    }

                    await chat.RequestStream.WriteAsync(new Message() { User = userName, Text = line });

                }

                await chat.RequestStream.CompleteAsync();
            }

            Console.WriteLine("Disconnecting");
            await channel.ShutdownAsync();
        }
    }
}
