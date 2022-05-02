using PPTask.Controllers.Model;
using System.Collections.Generic;

namespace PPTask.Repositories
{
    public interface ITagRepository
    {
        System.Threading.Tasks.Task AddTag(Tag tag);
        System.Threading.Tasks.Task<List<Tag>> GetTags();
        //System.Threading.Tasks.Task RemoveAllTags();
        System.Threading.Tasks.Task RemoveTag(int id);
    }
}
