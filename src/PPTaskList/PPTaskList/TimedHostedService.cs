using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PPTask
{
    public class TimedHostedService: IHostedService, IDisposable
    {
        public int executionCount = 0;
        //private ILogger<TimedHostedService> _logger;
        private Timer _timer = null!;

        //public ILogger<TimedHostedService> Logger { get => _logger; set => _logger = value; }

        //private TimedHostedService(ILogger<TimedHostedService> logger)
        //{
        //    //Logger = logger;
        //}
        public TimedHostedService() { }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            //_logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        public virtual void DoWork(object? state)
        {
            //var count = Interlocked.Increment(ref executionCount);

            //_logger.LogInformation(
                //"Timed Hosted Service is working. Count: {Count}", count); 
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            //_logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
