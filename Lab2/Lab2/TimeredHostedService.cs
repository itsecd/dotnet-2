using Lab2.Repositories;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lab2
{
    public class TimeredHostedService : IHostedService, IDisposable
    {
        private Timer _timer;

        private readonly IExecutorRepository _executorRepository;
        private readonly ITaskRepository _taskRepository;

        public TimeredHostedService(IExecutorRepository executorRepository,
            ITaskRepository taskRepository)
        {
            _executorRepository = executorRepository;
            _taskRepository = taskRepository;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _executorRepository.WriteToFile();
            _taskRepository.WriteToFile();
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}