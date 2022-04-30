using Lab2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab2.Repositories
{
    public interface ITagRepository
    {
        Task<int> AddTag(Tags tag);
        Task<List<Tags>> GetTags();
        Task RemoveAllTags();
        Task<int> RemoveTag(int id);
        Task SaveFile();
    }
}
