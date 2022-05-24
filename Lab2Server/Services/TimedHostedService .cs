using Lab2Server.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Lab2Server.Services
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private int _executionCount;
        private readonly ILogger<TimedHostedService> _logger;
        private Timer _timer;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public TimedHostedService(ILogger<TimedHostedService> logger, IUserRepository userRepository, IConfiguration configuration)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");
            _userRepository.ReadFromFile();
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(5));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref _executionCount);
            List<User> users = _userRepository.GetUsers();
            if (users != null)
            {
                foreach (var user in users)
                {
                    foreach (var reminder in user.ReminderList)
                    {
                        if (((reminder.DateTime - DateTime.Now).TotalMinutes < 10) && ((reminder.DateTime - DateTime.Now).TotalMinutes >= 0))
                        {
                            string key = "123";
                            string bot = _configuration.GetValue(key, "");
                            var botClient = new TelegramBotClient(bot);
                            botClient.SendTextMessageAsync(chatId: user.ChatId, text: $"{reminder.Name}: {reminder.Description}").Wait();
                            _logger.LogTrace("Messege was sent");
                        }
                    }
                }
            }
            _logger.LogInformation("Timed Hosted Service is working. Count: {Count}", count);
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
