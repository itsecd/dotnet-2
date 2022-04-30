using Lab2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab2.Repositories
{
    public interface IExecutorRepository
    {
        Task<List<Executor>> GetExecutors();
        Task<int> AddExecutor(Executor executor);
        Task RemoveAllExecutors();
        Task<int> RemoveExecutor(int id);
        Task SaveFile();
    }
}
