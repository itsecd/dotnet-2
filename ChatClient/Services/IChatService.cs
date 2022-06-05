using ChatClient.Model;
using System;
using System.Threading.Tasks;

namespace ChatClient.Services
{
    public interface IChatService
    {
        IObservable<string> UserJoined { get; }

        IObservable<MessageModel> MessageReceived { get; }

        Task Connect();

        Task<bool> Login(string? login);

        Task SendMessage(string receiver, string message);
    }
}
