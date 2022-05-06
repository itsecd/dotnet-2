using Lab2.Models;
using System.Collections.Generic;

namespace Lab2.Repositories
{
    public interface ITaskRepository
    {
        int AddTask(Task task);
        List<Task> GetTasks();
        void RemoveAllTasks();
        int RemoveTask(int id);
        int UpdateTask(int id, Task newTask);
        void WriteToFile();
    }
}
