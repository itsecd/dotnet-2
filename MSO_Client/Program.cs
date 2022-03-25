using System;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;

namespace MSO_Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:5000");
            var client = new GameNetwork.GameNetworkClient(channel);
            var reply = await client.CreateRoomAsync(new CreateRequest{PlayerName = "dimadivan", MaxPlayers = 32});
            Console.WriteLine(reply.RoomId);
            Console.ReadKey();
        }
    }
}
