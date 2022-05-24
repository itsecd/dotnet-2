using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using TelegramBot.Repository;

namespace TelegramBot.Services
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private int _executionCount;
        private Timer _timer;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TimedHostedService> _logger;
        private readonly IUsersRepository _usersRepository;

        public TimedHostedService(ILogger<TimedHostedService> logger, IConfiguration configuration, IUsersRepository usersRepository)
        {
            _executionCount = 0;
            _configuration = configuration;
            _usersRepository = usersRepository;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is running.");
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(2));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref _executionCount);
            var users = _usersRepository.GetUsers();
            var botClient = new TelegramBotClient(_configuration.GetValue<string>("TelegramBotKey"));
            foreach (var user in users)
            {
                foreach (var reminder in user.EventReminders.Where(reminder => (reminder.Time - DateTime.Now).TotalMinutes is < 5 and >= 0))
                {
                    botClient.SendTextMessageAsync(user.ChatId, $"{reminder.Name}: {reminder.Description}").Wait();
                    if (reminder.RepeatPeriod.TotalMinutes > 0)
                    {
                        reminder.Time += reminder.RepeatPeriod;
                    }
                    _logger.LogInformation($"Message to User {user.UserId} was sended");
                }
            }
            _logger.LogInformation(
                $"Timed Hosted Service is working. Count: {_executionCount}");
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
