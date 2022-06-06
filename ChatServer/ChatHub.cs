using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using ChatServer.Services;

namespace ChatServer
{
    public class ChatHub : Hub
    {
        private readonly object _lock = new();

        private readonly IChatService _chatService;

        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }
        
        public async Task<bool> Login(string login)
        {
            string[] logins;

            lock (_lock)
            {
                if (_chatService.LoginToIdContainsKey(login))
                    return false;

                logins = _chatService.LoginToIdArrayKeys();

                _chatService.LoginToIdAdd(login, Context.ConnectionId);
                _chatService.IdToLoginAdd(Context.ConnectionId, login);                
            }
            foreach (var existedLogin in logins)
                await Clients.Caller.SendAsync("UserJoined", existedLogin);

            await Clients.Others.SendAsync("UserJoined", login);
            return true;
        }

        public Task SendMessage(string receiver, string message)
        {
            var sender = ResolveCallerLogin();
            return Clients.Client(ResolveConnectionId(receiver)).SendAsync("MessageReceived", sender, message);
        }

        private string ResolveCallerLogin()
        {
            lock (_lock)
            {
                return _chatService.ResolveCallerLogin(Context.ConnectionId);
            }
        }

        private string ResolveConnectionId(string login)
        {
            lock (_lock)
            {
                return _chatService.ResolveConnectionId(login);
            }
        }
    }
}
