using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TelegramBot.Repository;

namespace TelegramBot.Services
{
    public class RepositoryHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<RepositoryHostedService> _logger;
        private Timer _timer;
        private readonly IUsersRepository _usersRepository;

        public RepositoryHostedService(ILogger<RepositoryHostedService> logger, IUsersRepository usersRepository)
        {
            _logger = logger;
            _usersRepository = usersRepository;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Repository Hosted Service is running");
            _usersRepository.ReadFromFile();
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(10));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _usersRepository.WriteToFile();
            _logger.LogInformation("Repository Hosted Service is working");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Repository Hosted Service is stopping");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
