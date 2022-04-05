using PPTaskList.Controllers.Model;
using System.Collections.Generic;

namespace PPTaskList.Repositories
{
    public interface IExecutorRepository
    {
        List<Executor> GetExecutors();
        void AddExecutor(Executor executor);
        void RemoveAllExecutors();
    }
}
