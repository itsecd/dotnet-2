using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using TelegramBot;
using TelegramBot.Model;
using TelegramBot.Repository;

namespace TelegramBot.Services
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<TimedHostedService> _logger;
        private Timer _timer;
        private readonly IUsersRepository _usersRepository;

        public TimedHostedService(ILogger<TimedHostedService> logger, ITelegramBotClient telegramBotClient, IUsersRepository usersRepository)
        {
            _telegramBotClient = telegramBotClient;
            _usersRepository = usersRepository;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is running.");
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(5));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);
            var users = _usersRepository.GetUsers();
            foreach (var user in users)
            {
                foreach (var reminder in user.EventReminders)
                {
                    if (((reminder.Time - DateTime.Now).TotalMinutes < 10) && ((reminder.Time - DateTime.Now).TotalMinutes >= 0))
                    {
                        _telegramBotClient.SendTextMessageAsync(chatId: user.ChatId, text: $"{reminder.Name}: {reminder.Description}").Wait();
                        _logger.LogTrace("Message was sended");
                    }
                }
            }
            _logger.LogInformation(
                $"Timed Hosted Service is working. Count: {executionCount}");
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
