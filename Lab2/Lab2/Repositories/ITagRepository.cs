using Lab2.Models;
using System.Collections.Generic;

namespace Lab2.Repositories
{
    public interface ITagRepository
    {
        void AddTag(Tags task);
        List<Tags> GetTags();
        void RemoveAllTags();
        void RemoveTag(int id);
    }
}
