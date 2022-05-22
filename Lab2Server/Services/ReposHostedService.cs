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
    public class ReposHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<ReposHostedService> _logger;
        private Timer _timer;

        private readonly IUserRepository _userRepository;
        public ReposHostedService(ILogger<ReposHostedService> logger, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Repos Hosted Service running.");
            _userRepository.ReadFromFile();
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(40));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _userRepository.WriteToFile();
            _logger.LogInformation("Timed Hosted Service is working.");
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
