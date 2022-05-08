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
        private int executionCount = 0;
        private readonly ILogger<ReposHostedService> _logger;
        private readonly IJSONUserRepository _userRepository;
        private readonly IJSONUserEventRepository _userEventRepository;
        private Timer _timer = null!;
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
            _logger.LogInformation("Timed Hosted Service running.");
            _userEventRepository.LoadData();
            _userRepository.LoadData();
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromSeconds(10));

            return Task.CompletedTask;
        }
        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);
            _userEventRepository.SaveData();
            _userRepository.SaveData();
            _logger.LogInformation(
            "Timed Hosted Service is working. Count: {Count}", count);
        }
        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            _userEventRepository.SaveData();
            _userRepository.SaveData();
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}