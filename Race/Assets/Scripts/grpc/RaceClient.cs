using Grpc.Core;
using RaceServer;
//using Grpc.Net.Client;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class RaceClient : MonoBehaviour
{
    public List<RaceServer.Event> Events { get; } = new();
    public string PlayerLogin { get; private set; } = "login";
    public bool SuccessLogin { get; private set; } = false;
    public bool OpponentFound { get; private set; }
    public string OpponentLogin { get; private set; }
    public Point Position { get; private set; } = new() { X = 0.5f, Y = -0.5f };
    public float Rotate { get; private set; } = 0.8f;
    public enum Result
    {
        None,
        Win,
        Lose
    }
    public Result ResultMatch { get; private set; } = Result.None;

    private readonly object _lock = new object();

    private Channel _channel;

    private AsyncDuplexStreamingCall<Request, RaceServer.Event> _stream;

    private Task _responseTask;

    [Header("Tags")]
    [SerializeField] private string createdTag;
    private void Awake()
    {
        GameObject obj = GameObject.FindWithTag(this.createdTag);
        if (obj != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            this.gameObject.tag = this.createdTag;
            DontDestroyOnLoad(this.gameObject);
        }


        var channel = new Channel("127.0.0.1:5000", ChannelCredentials.Insecure);
        {
            var client = new RaceService.RaceServiceClient(channel);
            _stream = client.Connect();

            _responseTask = Task.Run(async () =>
            {
                while (await _stream.ResponseStream.MoveNext(CancellationToken.None))
                {
                    var server_event = _stream.ResponseStream.Current;
                    Events.Add(server_event);

                    switch (server_event.EventCase)
                    {
                        case RaceServer.Event.EventOneofCase.Login:
                            SuccessLogin = server_event.Login.Success;
                            break;
                        case RaceServer.Event.EventOneofCase.FindOpponent:
                            OpponentLogin = server_event.FindOpponent.OpponentLogin;
                            OpponentFound = true;
                            break;
                        case RaceServer.Event.EventOneofCase.OpponentPosition:
                            Position = server_event.OpponentPosition.Position;
                            Rotate = server_event.OpponentPosition.Rotate;
                            break;
                        case RaceServer.Event.EventOneofCase.ResultGame:
                            ResultMatch = server_event.ResultGame.Win ? Result.Win : Result.Lose;
                            CloseConnection();
                            break;

                    }
                }
            });
        }
    }

    public async void Login()
    {
        var loginRequest = new LoginRequest { Login = PlayerLogin };
        var request = new Request { Login = loginRequest };
        await _stream.RequestStream.WriteAsync(request);
    }

    public async void FindOpponent()
    {
        var findOpponentRequest = new FindOpponentRequest();
        var request = new Request { FindOpponent = findOpponentRequest };
        await _stream.RequestStream.WriteAsync(request);
    }

    public async void Win()
    {
        var winGameRequest = new WinGameRequest();
        var request = new Request { WinGame = winGameRequest };
        await _stream.RequestStream.WriteAsync(request);
    }

    public async void ChangePosition(float x, float y, float rotate)
    {
        Point point = new Point { X = x, Y = y };
        var changePositionRequest = new ChangePositionRequest { Position = point, Rotate = rotate };
        var request = new Request { ChangePosition = changePositionRequest };
        await _stream.RequestStream.WriteAsync(request);
    }

    public async void CloseConnection()
    {
        var closeConnectionRequest = new CloseConnectRequest();
        var request = new Request { CloseConnect = closeConnectionRequest };
        await _stream.RequestStream.WriteAsync(request);
    }

    private async Task ShutDown(Channel channel)
    {

        Debug.Log("Enter ShutDown");
        await channel.ShutdownAsync();
        Debug.Log("ShutDown complited");

    }


    public void SetLogin(string login)
    {
        PlayerLogin = login;
    }

}