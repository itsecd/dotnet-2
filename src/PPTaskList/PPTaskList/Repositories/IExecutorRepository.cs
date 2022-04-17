using PPTask.Controllers.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PPTask.Repositories
{
    public interface IExecutorRepository
    {
        Task<List<Executor>> GetExecutors();
        System.Threading.Tasks.Task AddExecutor(Executor executor);
        System.Threading.Tasks.Task RemoveAllExecutors();
    }
}
