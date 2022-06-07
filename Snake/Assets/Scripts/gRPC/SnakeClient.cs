using Grpc.Core;
using Snake;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class SnakeClient : MonoBehaviour
{
    public List<Reply> Replies { get; } = new();
    public string PlayerLogin { get; private set; } = "login";
    public bool SuccessLogin { get; private set; } = false;

    public enum Result
    {
        None,
        Win,
        Lose
    }
    public Result ResultMatch { get; private set; } = Result.None;

    private readonly object _lock = new object();

    private Channel _channel;

    private AsyncDuplexStreamingCall<Request, Reply> _stream;

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
            var client = new Snake.Snake.SnakeClient(channel);
            _stream = client.Play();

            _responseTask = Task.Run(async () =>
            {
                while (await _stream.ResponseStream.MoveNext(CancellationToken.None))
                {
                    var server_event = _stream.ResponseStream.Current;
                    Replies.Add(server_event);

                    switch (server_event.ReplyCase)
                    {
                        case Snake.Reply.ReplyOneofCase.LoginReply:
                            SuccessLogin = server_event.LoginReply.Success;
                            break; 
                    }
                }
            });
        }

    }

    public async void Login()
    {
        var loginRequest = new LoginRequest { Login = PlayerLogin };
        var request = new Request { LoginRequest = loginRequest };
        await _stream.RequestStream.WriteAsync(request);
    }

    public async void SendResult(int score)
    {
        var sendResultRequest = new SendResultGame { Score = score };
        var request = new Request { SendResultGame = sendResultRequest };
        await _stream.RequestStream.WriteAsync(request);
    }

    public void SetLogin(string login)
    {
        PlayerLogin = login;
        Debug.Log($"login: {login}");
    }

}
