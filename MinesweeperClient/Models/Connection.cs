using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using MinesweeperClient;

namespace MinesweeperClient.Models
{
    public class Connection
    {
        string? _name;
        string? _address;
        GrpcChannel? _channel;
        Minesweeper.MinesweeperClient? _client;
        AsyncDuplexStreamingCall<PlayerMessage, ServerMessage>? _call = null;
        public async Task<bool> TryJoinAsync(string Name, string Address)
        {
            if (Name == null || Address == null)
                return false;
            _name = Name;
            _address = Address;
            try
            {
                _channel = GrpcChannel.ForAddress(_address);
                _client = new Minesweeper.MinesweeperClient(_channel);
                _call = _client.Join();
                await _call.RequestStream.WriteAsync(new PlayerMessage{Name = _name});
            }
            catch (Exception e)
            {
                _call = null;
                _client = null;
                _channel = null;
                Console.WriteLine($"[{e.Source}]\n{e.Message}");
                return false;
            }
            return true;
        }
        public async Task<bool> Leave()
        {
            if (_call == null || _channel == null)
                return false;
            await _call.RequestStream.WriteAsync(new PlayerMessage{Name=_name, State="leave"});
            _call.Dispose();
            _channel.Dispose();
            _call = null;
            _client = null;
            _channel = null;
            return true;
        }
        public bool IsConnected => _call != null;
    }
}