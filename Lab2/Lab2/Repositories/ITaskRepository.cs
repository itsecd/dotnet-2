using Lab2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab2.Repositories
{
    public interface ITaskRepository
    {
        Task<int> AddTask(TaskList task);
        Task<List<TaskList>> GetTasks();
        Task RemoveAllTasks();
        Task SaveFile();
        Task<int> RemoveTask(int id);
    }
}
