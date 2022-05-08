using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using TelegramBotServer.Repository;

namespace TelegramBotServer.Services
{
    public class EventWatcherHostedService : IHostedService
    {
        public ILogger<EventWatcherHostedService> _logger { get; set; }
        public ITelegramBotClient _bot { get; private set; }
        public IEventRepository _eventRepository { get; private set; }
        public INotificationSenderService _notificationSender { get; private set; }
        public Timer _timer { get; set; }

        public EventWatcherHostedService(ILogger<EventWatcherHostedService> logger, ITelegramBotClient bot,
            IEventRepository eventRepository, INotificationSenderService notificationSender)
        {
            _logger = logger;
            _bot = bot;
            _eventRepository = eventRepository;
            _notificationSender = notificationSender;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(CheckEvents, null, TimeSpan.Zero,
            TimeSpan.FromSeconds(60));
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
            foreach (var someEvent in events)
            {
                var rest = someEvent.Deadline.Subtract(DateTime.Now);
                if (rest <= TimeSpan.FromMinutes(someEvent.Reminder))
                {
                    _notificationSender.NotifyAsync(someEvent);
                }
            }
        }
    }
}
