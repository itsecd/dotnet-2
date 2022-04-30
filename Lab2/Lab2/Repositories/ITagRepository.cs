using Lab2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab2.Repositories
{
    public interface ITagRepository
    {
        int AddTag(Tags tag);
        List<Tags> GetTags();
        void RemoveAllTags();
        int RemoveTag(int id);
        int UpdateTag(int id, Tags newTag);
        
    }
}
