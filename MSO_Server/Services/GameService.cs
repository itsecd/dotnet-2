using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using System;

namespace MSO_Server
{
    public class GameService : Minesweeper.MinesweeperBase
    {
        private readonly ILogger<GameService> _logger;
        private readonly GameDatabase _data;
        public GameService(ILogger<GameService> logger, GameDatabase data)
        {
            _logger = logger;
            _data = data;
            _data.Load();
        }

        public override async Task Join(IAsyncStreamReader<PlayerMessage> requestStream, IServerStreamWriter<ServerMessage> responseStream, ServerCallContext context)
        {
            if (!await requestStream.MoveNext()) return;
            var initMessage = requestStream.Current;
            string playerName = initMessage.Name;
            // add player if new
            if (_data.TryAdd(playerName))
                await _data.DumpAsync();
            // join player
            _data.Join(playerName, responseStream);
            // pre game
            while(!_data.AllStates("ready"))
            {
                await requestStream.MoveNext();
                var message = requestStream.Current;
                switch (message.Text)
                {
                    case "players":
                        await _data.SendPlayers(playerName);
                        break;
                    case "ready":
                        _data.Ready(playerName);
                        break;
                    case "leave":
                        _data.Leave(playerName);
                        return;
                    default:
                        break;
                }
            }
            // in game
            while (!_data.AllStates("lobby"))
            {
                await requestStream.MoveNext();
                var message = requestStream.Current;
                switch (message.Text)
                {
                    case "players":
                        await _data.SendPlayers(playerName);
                        break;
                    case "leave":
                        _data.Leave(playerName);
                        return;
                    case "win":
                        await _data.Broadcast(new ServerMessage{Text = playerName, State = "win"}, playerName);
                        break;
                    case "lose":
                        await _data.Broadcast(new ServerMessage{Text = playerName, State = "lose"}, playerName);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}