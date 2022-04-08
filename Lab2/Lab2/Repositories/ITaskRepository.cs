using Lab2.Models;
using System.Collections.Generic;

namespace Lab2.Repositories
{
    public interface ITaskRepository
    {
        void AddTask(TaskList task);
        List<TaskList> GetTasks();
        void RemoveAllTasks();
    }
}
