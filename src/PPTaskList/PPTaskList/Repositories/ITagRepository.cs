using PPTask.Controllers.Model;
using System.Collections.Generic;

namespace PPTask.Repositories
{
    public interface ITagRepository
    {
        System.Threading.Tasks.Task AddTag(Tags tag);
        System.Threading.Tasks.Task<List<Tags>> GetTags();
        System.Threading.Tasks.Task RemoveAllTags();
        //System.Threading.Tasks.Task RemoveTag(int id);
    }
}
