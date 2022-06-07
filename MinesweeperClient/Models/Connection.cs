using System;
using System.Collections.Generic;
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
        public List<PlayerInfo> Players = new();
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
        public async Task UpdatePlayers()
        {
            if (_call == null || !IsConnected)
                return;
            try
            {
                await _call.RequestStream.WriteAsync(new GameMessage { Text = "players" });
                GameMessage msg = new();
                Players.Clear();
                while (true)
                {
                    await _call.ResponseStream.MoveNext();
                    msg = _call.ResponseStream.Current;
                    if (msg.Text != "end")
                    {
                        PlayerInfo info = new();
                        info.Name = msg.Name;
                        int win_count = int.Parse(msg.Text.Split('_')[0]);
                        int lose_count = int.Parse(msg.Text.Split('_')[1]);
                        int win_streak = int.Parse(msg.Text.Split('_')[2]);
                        info.PlayCount = win_count + lose_count;
                        info.WinCount = win_count;
                        info.WinStreak = win_streak;
                        Players.Add(info);
                    }
                    else
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[{e.Source}]\n{e.Message}");
            }
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
        public async Task<bool> Win()
        {
            if (_call == null)
                return false;
            await _call.RequestStream.WriteAsync(new GameMessage { Name = _name, Text = "win" });
            return true;
        }
        public async Task<bool> Lose()
        {
            if (_call == null)
                return false;
            await _call.RequestStream.WriteAsync(new GameMessage { Name = _name, Text = "lose" });
            return true;
        }
        public async Task<GameMessage> Peek()
        {
            if (await _call.ResponseStream.MoveNext())
                return _call.ResponseStream.Current;
            return new GameMessage { Text = "no" };
        }
    }
}