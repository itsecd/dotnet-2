using Lab2.Models;
using System.Collections.Generic;

namespace Lab2.Repositories
{
    public interface IExecutorRepository
    {
        List<Executor> GetExecutors();
        void AddExecutor(Executor executor);
        void RemoveAllExecutors();
        void RemoveExecutor(int id);
    }
}
