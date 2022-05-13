using Lab2Server.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Lab2Server.Services
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<TimedHostedService> _logger;
        private Timer _timer;

        private readonly IUserRepository _userRepository;
        public TimedHostedService(ILogger<TimedHostedService> logger, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(5));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);
            List<User> users = _userRepository.GetUsers();
            foreach (var user in users)
            {
                foreach (var reminder in user.ReminderList)
                {
                    if (((reminder.DateTime - DateTime.Now).TotalMinutes < 10) && ((reminder.DateTime - DateTime.Now).TotalMinutes >= 0))
                    {
                        var botClient = new TelegramBotClient("5258202735:AAFKlKSQ9G8xCh68C_dGHqyM92mrSfQknKo");    
                        botClient.SendTextMessageAsync(chatId: user.ChatId, text: $"{reminder.Name}: {reminder.Description}").Wait();
                        _logger.LogTrace("Messege was sended");
                    }
                }
            }    
            _logger.LogInformation(
                "Timed Hosted Service is working. Count: {Count}", count);
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
