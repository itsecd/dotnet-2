using PPTask.Controllers.Model;
using System.Collections.Generic;

namespace PPTask.Repositories
{
    /// <summary>
    /// Интерфейс репозиторий тегов 
    /// </summary>
    public interface ITagRepository
    {
        /// <summary>
        /// Метод добавления тега 
        /// </summary>
        /// <param name="tag">Тег</param>
        /// <returns> Task </returns>
        System.Threading.Tasks.Task AddTag(Tag tag);

        /// <summary>
        /// Метод получения всех тегов 
        /// </summary>
        /// <returns>Список тегов</returns>
        System.Threading.Tasks.Task<List<Tag>> GetTags();

        /// <summary>
        /// Метод удаления тега  
        /// </summary>
        /// <param name="id">Идентификатор тега</param>
        ///  <returns> Task </returns>
        System.Threading.Tasks.Task RemoveTag(int id);
    }
}
