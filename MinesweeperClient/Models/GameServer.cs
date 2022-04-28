using Grpc.Net.Client;
using Grpc.Core;
using System.Threading;
using System;
using Spectre.Console;
using System.Threading.Tasks;

namespace MinesweeperClient.Models
{
    public class GameServer
    {
        GrpcChannel? _channel;
        Minesweeper.MinesweeperClient? _client;
        AsyncDuplexStreamingCall<PlayerMessage, ServerMessage>? _call;
        public string? Name;
        public string? Address;
        public async Task<bool> Join()
        {
            try
            {
                if (Name == null || Address == null)
                    throw new NullReferenceException("Name or address is null!");
                _channel = GrpcChannel.ForAddress(Address);
                _client = new Minesweeper.MinesweeperClient(_channel);
                _call = _client.Join(cancellationToken: CancellationToken.None);
                await _call.RequestStream.WriteAsync(new PlayerMessage { Name = Name });
            }
            catch (Exception e)
            {
                AnsiConsole.MarkupLine($"[white on red][[{e.Source}]][/]\n{e.Message}");
                return false;
            }
            return true;
        }
        public void GetPlayerList() { }
        public void Win() { }
        public void Lose() { }
        public async Task<bool> Ready()
        {
            if (_call == null)
                return false;
            await _call.RequestStream.WriteAsync(new PlayerMessage { Name = Name, Text = "ready" });
            return true;
        }
        public async Task<bool> Leave()
        {
            if (_call == null || _channel == null)
                return false;
            await _call.RequestStream.WriteAsync(new PlayerMessage { Name = Name, Text = "leave" });
            await _channel.ShutdownAsync();
            _channel = null;
            _client = null;
            _call = null;
            return true;
        }
    }
}