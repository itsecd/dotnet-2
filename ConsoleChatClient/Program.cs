using ChatService;
using Grpc.Net.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleChatClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Room Name");
            var roomName = Console.ReadLine();
            Console.WriteLine("User Name");
            var userName = Console.ReadLine();
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Chat.ChatClient(channel);

            using (var chat = client.Create())
            {
                await chat.RequestStream.WriteAsync(new Message { RoomName = roomName, UserName = userName });
                Task.Delay(1000).Wait();
                using (var room = client.Join())
                {
                    _ = Task.Run(async () =>
                    {
                        while (await room.ResponseStream.MoveNext(cancellationToken: CancellationToken.None))
                        {
                            var response = room.ResponseStream.Current;
                            Console.WriteLine($"{response.UserName}: {response.Text}");
                        }
                    });

                    await room.RequestStream.WriteAsync(new Message { RoomName = roomName, UserName = userName, Text = "${userName} has joined the room" });

                    string line;
                    while ((line = Console.ReadLine()) != null)
                    {
                        if (line == "bye")
                        {
                            break;
                        }
                        await room.RequestStream.WriteAsync(new Message { RoomName = roomName, UserName = userName, Text = line });
                    }

                    await room.RequestStream.CompleteAsync();
                }
                Console.WriteLine("Disconnect");
                await channel.ShutdownAsync();
            }
        }
    }
}
