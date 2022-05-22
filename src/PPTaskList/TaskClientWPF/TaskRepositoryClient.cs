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

        public Task<Executor> GetExecutorAsync(int id)
        {
            return _client.Executor2Async(id);
        }

        public Task PostExecutorAsync(ExecutorDto executor)
        {
            return _client.ExecutorAsync(executor);
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

        public Task<ICollection<Tag>> GetTagsAsync()
        {
            return _client.TagAllAsync();
        }

        public Task<Tag> GetTagAsync(int id)
        {
            return _client.Tag2Async(id);
        }

        public Task PostTagAsync(TagDto tag)
        {
            return _client.TagAsync(tag);
        }

        public Task UpdateTagAsync(int id, TagDto tag)
        {
            return _client.Tag3Async(id, tag);
        }

        public Task RemoveTagAsync(int id)
        {
            return _client.Tag4Async(id);
        }

        public Task<ICollection<TaskDto>> GetTasksAsync()
        {
            return _client.TaskAllAsync();
        }

        public Task<TaskDto> GetTaskAsync(int id)
        {
            return _client.Task2Async(id);
        }

        public Task PostTaskAsync(TaskDto task)
        {
            return _client.TaskAsync(task);
        }

        public Task UpdateTaskAsync(int id, TaskDto task)
        {
            return _client.Task3Async(id, task);
        }

        public Task RemoveTaskAsync(int id)
        {
            return _client.Task4Async(id);
        }
    }
}
