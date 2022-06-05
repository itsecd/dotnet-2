using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.SignalR;

namespace ChatServer
{
    public class ChatHub : Hub
    {
        private readonly object _lock = new();
        private readonly Dictionary<string, string> _loginToId = new();
        private readonly Dictionary<string, string> _idToLogin = new();

        public async Task<bool> Login(string login)
        {
            string[] logins;

            lock (_lock)
            {
                if (_loginToId.ContainsKey(login))
                    return false;

                logins = _loginToId.Keys.ToArray();

                _loginToId.Add(login, Context.ConnectionId);
                _idToLogin.Add(Context.ConnectionId, login);
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
                return _idToLogin[Context.ConnectionId];
            }
        }

        private string ResolveConnectionId(string login)
        {
            lock (_lock)
            {
                return _loginToId[login];
            }
        }

    }
}
