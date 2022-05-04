using Lab2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab2.Repositories
{
    public interface IExecutorRepository
    {
        List<Executor> GetExecutors();
        int AddExecutor(Executor executor);
        void RemoveAllExecutors();
        int RemoveExecutor(int id);
        int UpdateExecutor(int id, Executor executor);
        void WriteToFile();
    }
}
