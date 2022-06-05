using System.Linq;
using System.Threading.Tasks;

using Grpc.Core;

using SudokuServer.Services;

namespace SudokuServer.Models
{
    public class GameSession
    {
        private readonly Player _player;
        private readonly Field _field;
        private readonly IAsyncStreamReader<Request> _requestStream;
        private readonly IServerStreamWriter<Event> _responseStream;

        public GameSession(Player player, IAsyncStreamReader<Request> requestStream, IServerStreamWriter<Event> responseStream)
        {
            _player = player;
            _requestStream = requestStream;
            _responseStream = responseStream;
            _field = new FieldGeneratorService().GenerateField();
        }

        public async Task Start()
        {
            await SendPlayEvent();

            while (await _requestStream.MoveNext())
            {
                if (_requestStream.Current.RequestCase == Request.RequestOneofCase.Turn)
                {
                    var point = _requestStream.Current.Turn.Point;
                    if (!Check(point))
                    {
                        await SendTurnEvent(false);
                    }
                    else
                    {
                        _field[point.X, point.Y] = point.Value;
                        await SendTurnEvent(true);
                        if (_field.Points.Count == _field.Size)
                            await SendWinEvent();
                    }
                }
            }

        }



        private bool Check(Point point)
        {
            for (int i=0; i < 9; ++i)
            {
                if (i == point.X)
                    continue;
                if (_field[i, point.Y] == point.Value)
                    return false;
            }

            for (int i = 0; i < 9; ++i)
            {
                if (i == point.Y)
                    continue;
                if (_field[point.X, i] == point.Value)
                    return false;
            }

            return true;
        }

        private async Task SendPlayEvent()
        {
            var playEvent = new PlayEvent();
            playEvent.Points.Add(_field.Points.ToArray());
            await _responseStream.WriteAsync(new Event() { Play = playEvent });
        }
        private async Task SendTurnEvent(bool success)
        {
            var turnEvent = new TurnEvent() { Success = success};
            await _responseStream.WriteAsync(new Event() { Turn = turnEvent });
        }
        private async Task SendWinEvent()
        {
            await _responseStream.WriteAsync(new Event() { Win = new WinEvent() });
        }

        

    }
}
