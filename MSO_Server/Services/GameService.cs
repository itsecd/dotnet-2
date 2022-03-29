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
            _data.TryAdd(playerName);
            _data.Dump();
            // join player
            _data.Join(playerName, responseStream);
            // pre game
            while(true)
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
        }
    }
}