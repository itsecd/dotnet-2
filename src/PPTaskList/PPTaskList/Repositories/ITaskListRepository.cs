using PPTaskList.Controllers.Model;
using System.Collections.Generic;

namespace PPTaskList.Repositories
{
    public interface ITaskListRepository
    {
        void AddTask(TaskList task);
        List<TaskList> GetTasks();
        void RemoveAllTasks();
    }
}
