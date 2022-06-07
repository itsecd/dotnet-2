using System;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

using Gomoku;

using Grpc.Core;
using Grpc.Net.Client;

namespace GomokuClient
{

    public sealed class Client
    {
        public Subject<LoginReply> LoginSubject { get; private set; } = new();
        public Subject<EndGameReply> EndGameSubject { get; private set; } = new();
        public Subject<MakeTurnReply> MakeTurnSubject { get; private set; } = new();
        public Subject<FindOpponentReply> FindOpponentSubject { get; private set; } = new();
        private readonly GrpcChannel _channel;
        private readonly AsyncDuplexStreamingCall<Request, Reply> _stream;
        private readonly Gomoku.Gomoku.GomokuClient _client;
        public bool IsFirstPlayer { get; set; }
        public bool ActivePlayer { get; set; } 
        public string Login { get; private set; } = string.Empty;
        public string OpponentLogin { get; private set; } = string.Empty;

        public Client()
        {
            _channel = GrpcChannel.ForAddress("http://localhost:5000");
            _client = new Gomoku.Gomoku.GomokuClient(_channel);
            _stream = _client.Play();
            Task.Run(ReadEvents);
        }

        public async Task LoginRequest(string login)
        {
            Login = login;
            var loginRequest = new LoginRequest { Login = login };
            var request = new Request { LoginRequest = loginRequest };
            await _stream.RequestStream.WriteAsync(request);
        }

        public async Task FindOpponentRequest()
        {
            var findOpponentRequest = new FindOpponentRequest();
            var request = new Request { FindOpponentRequest = findOpponentRequest };
            await _stream.RequestStream.WriteAsync(request);
        }

        public async Task MakeTurnRequest(Point point)
        {
            var makeTurnRequest = new MakeTurnRequest { Point = point };
            var request = new Request { MakeTurnRequest = makeTurnRequest };
            await _stream.RequestStream.WriteAsync(request);
        }

        private async Task ReadEvents()
        {
            //try
            //{
            var stream = _stream.ResponseStream;
            while (await stream.MoveNext(CancellationToken.None))
            {
                switch (stream.Current.ReplyCase)
                {
                    case Reply.ReplyOneofCase.None:
                        throw new InvalidOperationException();

                    case Reply.ReplyOneofCase.LoginReply:
                        LoginSubject.OnNext(stream.Current.LoginReply);
                        break;

                    case Reply.ReplyOneofCase.FindOpponentReply:
                        ActivePlayer = stream.Current.FindOpponentReply.YourTurn;
                        IsFirstPlayer = stream.Current.FindOpponentReply.YourTurn;
                        OpponentLogin = stream.Current.FindOpponentReply.Login;
                        FindOpponentSubject.OnNext(stream.Current.FindOpponentReply);
                        break;

                    case Reply.ReplyOneofCase.MakeTurnReply:
                        ActivePlayer = stream.Current.MakeTurnReply.YourTurn;
                        MakeTurnSubject.OnNext(stream.Current.MakeTurnReply);
                        break;

                    case Reply.ReplyOneofCase.EndGameReply:
                        EndGameSubject.OnNext(stream.Current.EndGameReply);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            //}
            //catch
            //{
            //    OnDisconnected();
            //}
        }

        //private void OnDisconnected()
        //{
        //    lock (_disconnectedSubject)
        //    {
        //        if (_alreadyDisconnected)
        //            return;

        //        _disconnectedSubject.OnNext(Unit.Default);
        //        _alreadyDisconnected = true;
        //    }
        //}
    }
}
