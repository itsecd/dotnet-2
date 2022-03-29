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
        public override async Task CreateRoom(IAsyncStreamReader<PlayerMessage> requestStream, IServerStreamWriter<ServerMessage> responseStream, ServerCallContext context)
        {
            if (!await requestStream.MoveNext()) return;

            // create room
            var playerMessage = requestStream.Current;
            int res = _data.Create(playerMessage.Name, playerMessage.State);
            await responseStream.WriteAsync(new ServerMessage{Text = "new room?", State = res});
            _data.Dump();

            // pre game
            while (true)
            {
                await requestStream.MoveNext();
                playerMessage = requestStream.Current;
                if (playerMessage.Text == "get_players")
                    await responseStream.WriteAsync(new ServerMessage{Text = "not implemented", State = 0});
                if (playerMessage.Text == "leave")
                {
                    await responseStream.WriteAsync(new ServerMessage{Text = "OK", State = 0});
                    break;
                }
            }
        }
        public override Task JoinRoom(IAsyncStreamReader<PlayerMessage> requestStream, IServerStreamWriter<ServerMessage> responseStream, ServerCallContext context)
        {
            return base.JoinRoom(requestStream, responseStream, context);
        }
    }
}