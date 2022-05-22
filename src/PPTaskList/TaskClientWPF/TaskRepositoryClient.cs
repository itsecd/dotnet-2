using TaskClientWPF.Properties;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace TaskClientWPF
{
    public class TaskRepositoryClient
    {
        private readonly TaskClient _client;

        public TaskRepositoryClient()
        {
            var httpClient = new HttpClient();
            var baseUrl = Settings.Default.OpenApiClient;
            _client = new TaskClient(baseUrl,httpClient);
        }

        public Task<ICollection<Executor>> GetExecutorsAsync()
        {
            return _client.ExecutorAllAsync();
        }

        public Task PostExecutorAsync(ExecutorDto executor)
        {
            return _client.ExecutorAsync(executor);
        }

        public Task<Executor> GetExecutorAsync(int id)
        {
            return _client.Executor2Async(id);
        }

        public Task UpdateExecutorAsync(int id, ExecutorDto executor)
        {
            return _client.Executor3Async(id, executor);
        }

        public Task RemoveExecutorAsync(int id)
        {
            return _client.Executor4Async(id);
        }

        public Task<ICollection<TaskDto>> GetExecutorTasksAsync(int id)
        {
            return _client.TasksAsync(id);
        }
    }
}
