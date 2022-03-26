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
            var reply1 = await client.CreateRoomAsync(new CreateInfo{PlayerName = "dimadivan", PlayersMax = 5});
            Console.WriteLine(reply1.RoomId);
            // var reply2 = await client.JoinRoomAsync(new RoomRequest{PlayerName = "dimadivan", RoomId = 111});
            // Console.WriteLine(reply2.Connected);
            // var reply3 = await client.LeaveRoomAsync(new RoomRequest{PlayerName = "dimadivan", RoomId = 111});
            // Console.WriteLine(reply3.Connected);
            Console.ReadKey();
        }
    }
}
