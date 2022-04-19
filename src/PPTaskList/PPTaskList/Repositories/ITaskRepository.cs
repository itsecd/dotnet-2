using PPTask.Controllers.Model;
using System.Collections.Generic;

namespace PPTask.Repositories
{
    public interface ITaskRepository
    {
        System.Threading.Tasks.Task AddTask(Task task);
        System.Threading.Tasks.Task<List<Task>> GetTasks();
        System.Threading.Tasks.Task RemoveTask(int id);
        System.Threading.Tasks.Task RemoveAllTasks();
    }
}
