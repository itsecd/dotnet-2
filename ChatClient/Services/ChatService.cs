using ChatClient.Model;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace ChatClient.Services
{
    public class ChatService : IChatService
    {
        public IObservable<string> UserJoined => _userJoined;

        public IObservable<MessageModel> MessageReceived => _messageReceived;

        public async Task Connect()
        {
            _connection = new HubConnectionBuilder()
               .WithUrl("http://localhost:5000/chatroom")
               .Build();

            _connection.On<string>(nameof(UserJoined), message => _userJoined.OnNext(message));
            _connection.On<string, string>(nameof(MessageReceived), (sender, message) => _messageReceived.OnNext(new MessageModel(sender, message)));

            await _connection.StartAsync();
        }

        public Task<bool> Login(string? login)
        {
            return Connection.InvokeAsync<bool>("Login", login);
        }

        public async Task SendMessage(string receiver, string message)
        {
            await Connection.InvokeAsync("SendMessage", receiver, message);
        }

        private HubConnection Connection => _connection ?? throw new InvalidOperationException();

        private HubConnection? _connection;
        private Subject<string> _userJoined = new();
        private Subject<MessageModel> _messageReceived = new();
    }
}
