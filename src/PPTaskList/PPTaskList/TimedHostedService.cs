using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using PPTask.Repositories;

namespace PPTask
{
    public class TimedHostedService: IHostedService, IDisposable
    {
        private Timer _timer;

        private readonly IExecutorRepository _executorRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ITaskRepository _taskRepository;

        public TimedHostedService(IExecutorRepository executorRepository,
            ITagRepository tagRepository,
            ITaskRepository taskRepository)
        {
            _executorRepository = executorRepository;
            _tagRepository = tagRepository;
            _taskRepository = taskRepository;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            await _executorRepository.WriteToFileAsync();
            await _tagRepository.WriteToFileAsync();
            await _taskRepository.WriteToFileAsync();
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
