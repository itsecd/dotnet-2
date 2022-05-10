using System;
using System.Threading;
using System.Threading.Tasks;
using Server.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Server.Services
{
    public class ReposHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<ReposHostedService> _logger;
        private readonly IJSONUserRepository _userRepository;
        private readonly IJSONUserEventRepository _userEventRepository;
        private Timer _timer;

        public ReposHostedService(ILogger<ReposHostedService> logger,
                                    IJSONUserRepository userRepository,
                                    IJSONUserEventRepository userEventRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _userEventRepository = userEventRepository;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Repos Hosted Service running.");
            _userEventRepository.LoadData();
            _userRepository.LoadData();
            _timer = new Timer(SaveRepositories, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));

            return Task.CompletedTask;
        }
        
        private void SaveRepositories(object state)
        {
            _userEventRepository.SaveData();
            _userRepository.SaveData();
            _logger.LogInformation("Repos Hosted Service is working.");
        }
        
        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Repos Hosted Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            _userEventRepository.SaveData();
            _userRepository.SaveData();
            return Task.CompletedTask;
        }
        
        public void Dispose() => _timer?.Dispose();
    }
}