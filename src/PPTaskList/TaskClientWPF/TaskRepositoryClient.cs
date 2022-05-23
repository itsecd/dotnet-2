using TaskClientWPF.Properties;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Lab2TaskClient
{
    public class TaskRepositoryClient
    {
        private readonly Client _client;

        public TaskRepositoryClient()
        {
            var httpClient = new HttpClient();
            var baseUrl = Settings.Default.OpenApiClient;
            _client = new Client(baseUrl,httpClient);
        }

        /// <summary>
        /// Метод получения исполнителей
        /// </summary>
        /// <returns>Исполнители</returns>
        public Task<ICollection<Executor>> GetExecutorsAsync()
        {
            return _client.ExecutorAllAsync();
        }

        /// <summary>
        /// Метод получения исполнителя по идентификатору 
        /// </summary>
        /// <param name="id">Идентификатор исполнителя</param>
        /// <returns>Исполнитель</returns>
        public Task<Executor> GetExecutorAsync(int id)
        {
            return _client.Executor2Async(id);
        }

        /// <summary>
        /// Метод получения всех задач для конкретного исполнителя 
        /// </summary>
        /// <param name="id">Идентификатор исполнителя</param>
        /// <returns>Задачи</returns>
        public Task<ICollection<TaskDto>> GetExecutorTasksAsync(int id)
        {
            return _client.TasksAsync(id);
        }

        /// <summary>
        /// Метод добавления исполнителя 
        /// </summary>
        /// <param name="value">Новый исполнитель</param>
        public System.Threading.Tasks.Task PostExecutorAsync(ExecutorDto executor)
        {
            return _client.ExecutorAsync(executor);
        }

        /// <summary>
        /// Метод замены исполнителя 
        /// </summary>
        /// <param name="value">Новый исполнитель</param>
        /// /// <param name="id">Идентификатор заменяемого исполнителя</param>
        public System.Threading.Tasks.Task UpdateExecutorAsync(int id, ExecutorDto executor)
        {
            return _client.Executor3Async(id, executor);
        }

        /// <summary>
        /// Метод удаления исполнителя 
        /// </summary>
        /// <param name="id">Идентификатор удаляемого исполнителя</param>
        public System.Threading.Tasks.Task RemoveExecutorAsync(int id)
        {
            return _client.Executor4Async(id);
        }

        /// <summary>
        /// Метод получения тегов
        /// </summary>
        /// <returns>Теги</returns>
        public Task<ICollection<Tag>> GetTagsAsync()
        {
            return _client.TagAllAsync();
        }

        /// <summary>
        /// Метод получения тега по идентификатору 
        /// </summary>
        /// <param name="id">Идентификатор тега</param>
        /// <returns>Тег</returns>
        public Task<Tag> GetTagAsync(int id)
        {
            return _client.Tag2Async(id);
        }

        /// <summary>
        /// Метод добавления тега 
        /// </summary>
        /// <param name="value">Новый тег</param>
        public System.Threading.Tasks.Task PostTagAsync(TagDto tag)
        {
            return _client.TagAsync(tag);
        }

        /// <summary>
        /// Метод замены тега 
        /// </summary>
        /// <param name="value">Новый тег</param>
        /// /// <param name="id">Идентификатор заменяемого тега</param>
        public System.Threading.Tasks.Task UpdateTagAsync(int id, TagDto tag)
        {
            return _client.Tag3Async(id, tag);
        }

        /// <summary>
        /// Метод удаления тега 
        /// </summary>
        /// <param name="id">Идентификатор удаляемого тега</param>
        public System.Threading.Tasks.Task RemoveTagAsync(int id)
        {
            return _client.Tag4Async(id);
        }

        /// <summary>
        /// Метод получения задач
        /// </summary>
        /// <returns>Теги</returns>
        public Task<ICollection<Task>> GetTasksAsync()
        {
            return _client.TaskAllAsync();
        }

        /// <summary>
        /// Метод получения задачи по идентификатору  
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <returns>Задача</returns>
        public Task<TaskDto> GetTaskAsync(int id)
        {
            return _client.Task2Async(id);
        }

        /// <summary>
        /// Метод добавления задачи 
        /// </summary>
        /// <param name="value">Новая задача</param>
        public System.Threading.Tasks.Task PostTaskAsync(TaskDto task)
        {
            return _client.TaskAsync(task);
        }

        /// <summary>
        /// Метод замены задачи 
        /// </summary>
        /// <param name="value">Новая задача</param>
        /// /// <param name="id">Идентификатор заменяемой задачи</param>
        public System.Threading.Tasks.Task UpdateTaskAsync(int id, TaskDto task)
        {
            return _client.Task3Async(id, task);
        }

        /// <summary>
        /// Метод удаления задачи 
        /// </summary>
        /// <param name="id">Идентификатор удаляемой задачи</param>
        public System.Threading.Tasks.Task RemoveTaskAsync(int id)
        {
            return _client.Task4Async(id);
        }
    }
}
