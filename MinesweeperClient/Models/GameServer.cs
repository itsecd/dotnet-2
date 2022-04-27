using Grpc.Net.Client;
using Grpc.Core;
using System.Threading;
using System;
using MinesweeperClient;

namespace MinesweeperClient.Models
{
    public class GameServer
    {
        GrpcChannel? _channel;
        Minesweeper.MinesweeperClient? _client;
        public string? Name;
        public string? Address;
        public bool TryConnect()
        {
            if (Name == null || Address == null)
                return false;
            try
            {
                _channel = GrpcChannel.ForAddress(Address);
                _client = new Minesweeper.MinesweeperClient(_channel);
                AsyncDuplexStreamingCall<PlayerMessage, ServerMessage> tmp = _client.Join(cancellationToken: CancellationToken.None);
                Console.WriteLine(tmp.GetStatus());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }
    }
}