using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TaskClient.Properties;

namespace TaskClient
{
    public class TaskRepositoryClient
    {
        private readonly Client _openApiClient;

        public TaskRepositoryClient()
        {
            var httpClient = new HttpClient();
            var baseUrl = Settings.Default.OpenApiService;
            _openApiClient = new Client(baseUrl, httpClient);
        }

        /// <summary>
        /// Метод получения исполнителей задачи
        /// </summary>
        /// <returns>Исполнители</returns>
        public Task<ICollection<Executor>> GetExecutorsAsync()
        {
            return _openApiClient.ExecutorAllAsync();
        }

        /// <summary>
        /// Метод получения исполнителя по идентификатору 
        /// </summary>
        /// <param name="id">Идентификатор исполнителя</param>
        /// <returns>Исполнитель</returns>
        public System.Threading.Tasks.Task<Executor> GetExecutorAsync(int id)
        {
            return _openApiClient.Executor3Async(id);
        }

        /// <summary>
        /// Добавление исполнителя задачи
        /// </summary>
        /// <param name="executor">Новый исполнитель задач</param>
        public System.Threading.Tasks.Task PostExecutorAsync(Executor executor)
        {
            return _openApiClient.ExecutorAsync(executor);
        }

        /// <summary>
        /// Метод замены исполнителя 
        /// </summary>
        /// <param name="executor">Новый исполнитель</param>
        /// <param name="id">Идентификатор заменяемого исполнителя</param>
        public System.Threading.Tasks.Task PutExecutorAsync(int id, Executor executor)
        {
            return _openApiClient.Executor4Async(id, executor);
        }

        /// <summary>
        /// Метод удаления исполнителя 
        /// </summary>
        /// <param name="id">Идентификатор удаляемого исполнителя</param>
        public System.Threading.Tasks.Task DeleteExecutorAsync(int id)
        {
            return _openApiClient.Executor5Async(id);
        }

        /// <summary>
        /// Метод удаления всех исполнителей
        /// </summary>
        public System.Threading.Tasks.Task DeleteAllExecutorAsync()
        {
            return _openApiClient.Executor2Async();
        }

        /// <summary>
        /// Метод получения задач
        /// </summary>
        /// <returns>Теги</returns>
        public Task<ICollection<Task>> GetTasksAsync()
        {
            return _openApiClient.TaskAllAsync();
        }

        /// <summary>
        /// Метод получения задачи по идентификатору  
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <returns>Задача</returns>
        public Task<Task> GetTaskAsync(int id)
        {
            return _openApiClient.Task3Async(id);
        }

        /// <summary>
        /// Метод добавления задачи 
        /// </summary>
        /// <param name="task">Новая задача</param>
        public System.Threading.Tasks.Task PostTaskAsync(Task task)
        {
            return _openApiClient.TaskAsync(task);
        }

        /// <summary>
        /// Метод замены задачи 
        /// </summary>
        /// <param name="task">Новая задача</param>
        /// <param name="id">Идентификатор заменяемой задачи</param>
        public System.Threading.Tasks.Task PutTaskAsync(int id, Task task)
        {
            return _openApiClient.Task4Async(id, task);
        }

        /// <summary>
        /// Метод удаления задачи 
        /// </summary>
        /// <param name="id">Идентификатор удаляемой задачи</param>
        public System.Threading.Tasks.Task RemoveTaskAsync(int id)
        {
            return _openApiClient.Task5Async(id);
        }

        /// <summary>
        /// Метод удаления всех исполнителей
        /// </summary>
        public System.Threading.Tasks.Task DeleteAllTaskAsync()
        {
            return _openApiClient.Task2Async();
        }

        /// <summary>
        /// Метод получения всех тэгов
        /// </summary>
        public System.Threading.Tasks.Task<ICollection<Tags>> GetAllProducts(int id)
        {
            return _openApiClient.TagsAllAsync(id);
        }

        /// <summary>
        /// Добавление тэга к определенной задаче
        /// </summary>
        /// /// <param name="id">Идентификатор задачи</param>
        /// <param name="tag">Новый тэг</param>
        public System.Threading.Tasks.Task<int> AddTag(int id, Tags tag)
        {
            return _openApiClient.TagsAsync(id, tag);
        }

        /// <summary>
        /// Удаление тэга,относящегося к определенной задаче
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        public System.Threading.Tasks.Task<int> DeleteTag(int id)
        {
            return _openApiClient.Tags2Async(id);
        }

        /// <summary>
        /// Получение определенного  тэга, относящегося к определенной задаче
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <param name="num">Номер тэга</param>
        public System.Threading.Tasks.Task<Tags> GetTag(int id, int num)
        {
            return _openApiClient.Tags3Async(id, num);
        }

        /// <summary>
        /// Удаление определенного  тэга, относящегося к определенной задаче
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        ///<param name="num">Номер тэга</param>
        public System.Threading.Tasks.Task<int> DeleteTag(int id, int num)
        {
            return _openApiClient.Tags4Async(id, num);
        }

        /// <summary>
        /// Изменение определенного  тэга, относящегося к определенной задаче
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <param name="num">Номер тэга</param>
        /// <param name="newTag">Новый тэг</param>
        public System.Threading.Tasks.Task<int> UpdateProduct(int id, int num, Tags newTag)
        {
            return _openApiClient.Tags5Async(id, num, newTag);
        }
    }
}
