using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TelegramBotServer.Repository;

namespace TelegramBotServer.Services
{
    public class EventWatcherHostedService : IHostedService
    {
        private ILogger<EventWatcherHostedService> _logger { get; set; }
        private IEventRepository _eventRepository { get; set; }
        private INotificationSenderService _notificationSender { get; set; }
        private Timer? _timer;

        public EventWatcherHostedService(ILogger<EventWatcherHostedService> logger, IEventRepository eventRepository,
            INotificationSenderService notificationSender)
        {
            _logger = logger;
            _eventRepository = eventRepository;
            _notificationSender = notificationSender;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(CheckEvents, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private void CheckEvents(object? state)
        {
            var events = _eventRepository.GetEvents();
            if (events is null)
                return;
            foreach (var someEvent in events)
            {
                var rest = someEvent.Deadline.Subtract(DateTime.Now);
                if (rest <= TimeSpan.FromMinutes(someEvent.Reminder) && !someEvent.Notified)
                {
                    _notificationSender.NotifyAsync(someEvent);
                    someEvent.Notified = true;
                    _eventRepository.ChangeEvent(someEvent.Id, someEvent);
                }
            }
        }
    }
}
