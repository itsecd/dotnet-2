using System;
using System.Linq;
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
        AsyncDuplexStreamingCall<GameMessage, GameMessage>? _call = null;
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
                await _call.RequestStream.WriteAsync(new GameMessage { Name = _name });
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
            await _call.RequestStream.WriteAsync(new GameMessage { Name = _name, Text = "leave" });
            _call.Dispose();
            _channel.Dispose();
            _call = null;
            _client = null;
            _channel = null;
            return true;
        }
        public async Task<PlayerInfo[]> GetPlayers()
        {
            PlayerInfo[] res = new PlayerInfo[0];
            if (_call == null)
                return res;
            await _call.RequestStream.WriteAsync(new GameMessage { Name = _name, Text = "players" });
            while (true)
            {
                await _call.ResponseStream.MoveNext();
                var message = _call.ResponseStream.Current;
                var stats = message.Text.Split('_');
                if (message.Text != "end")
                {
                    res.Append(new PlayerInfo
                        {
                            Name = message.State == "ready" ? $"[{message.Name}]" : message.Name,
                            WinCount = int.Parse(stats[0]),
                            PlayCount = int.Parse(stats[0]) + int.Parse(stats[1]),
                            WinStreak = int.Parse(stats[2])
                        }
                    );
                }
                if (message.Text == "end")
                    break;
            }
            return res;
        }
        public bool IsConnected => _call != null;
        public async Task<bool> Ready()
        {
            if (_call == null)
                return false;
            await _call.RequestStream.WriteAsync(new GameMessage { Name = _name, Text = "ready" });
            while (true)
            {
                await _call.ResponseStream.MoveNext();
                if (_call.ResponseStream.Current.Text == "start")
                    break;
            }
            Console.WriteLine("Пора начинать игру!");
            return true;
        }
    }
}