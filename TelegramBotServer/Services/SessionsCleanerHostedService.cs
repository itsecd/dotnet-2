using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TelegramBotServer.Services
{
    public class SessionsCleanerHostedService : IHostedService
    {
        private Timer? _timer;
        private SubscriberSessions _sessions;
        private readonly TimeSpan _sessionLifetime = TimeSpan.FromMinutes(30);
        private readonly TimeSpan _checkDelay = TimeSpan.FromMinutes(30);

        public SessionsCleanerHostedService(SubscriberSessions sessions)
        {
            _sessions = sessions;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(CleanSessions, null, TimeSpan.Zero, _checkDelay);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private void CleanSessions(object? state)
        {
            foreach (var s in _sessions.Sessions)
            {
                if (DateTime.Now.Subtract(s.Value.CreationTime) > _sessionLifetime)
                {
                    _sessions.Sessions.Remove(s.Key);
                }
            }
        }
    }
}
