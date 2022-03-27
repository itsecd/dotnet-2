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
            var roomInfo = await client.CreateRoomAsync(new CreateInfo{PlayerName = "dimadivan", PlayersMax = 5});
            Console.ReadKey();
            int newId = roomInfo.RoomId;
            var connection = await client.JoinRoomAsync(new RoomRequest{PlayerName = "gromimolnia", RoomId = newId});
            Console.ReadKey();
            connection = await client.JoinRoomAsync(new RoomRequest{PlayerName = "grey_wizard", RoomId = newId});
            Console.ReadKey();
            connection = await client.JoinRoomAsync(new RoomRequest{PlayerName = "divanchik", RoomId = newId});
            Console.ReadKey();
            var reply3 = await client.LeaveRoomAsync(new RoomRequest{PlayerName = "grey_wizard", RoomId = newId});
            Console.ReadKey();
        }
    }
}
