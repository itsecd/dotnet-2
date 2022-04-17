using Lab2.Models;
using System.Collections.Generic;

namespace Lab2.Repositories
{
    public interface ITagRepository
    {
        int AddTag(Tags task);
        List<Tags> GetTags();
        void RemoveAllTags();
        int RemoveTag(int id);
        void SaveFile();
    }
}
