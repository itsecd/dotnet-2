using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PPTask
{
    public class TimedHostedService: IHostedService, IDisposable
    {
        private Timer _timer = null!;

        private IExecutorRepository = _executorRepository;
        private ITagRepository = _tagRepository;
        private ITaskRepository = _taskRepository;

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

        private async void DoWork(object? state)
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
