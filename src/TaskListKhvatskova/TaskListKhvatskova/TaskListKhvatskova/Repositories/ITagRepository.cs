using System.Collections.Generic;
using TaskListKhvatskova.Models;

namespace TaskListKhvatskova.Repositories
{
    /// <summary>
    /// Интерфейс репозитория тэгов  
    /// </summary>
    public interface ITagRepository
    {
        /// <summary>
        /// Мeтод добавления тэга 
        /// </summary>
        /// <param name="tag">Тэг</param>
        ///<returns>Идентификатор тэга</returns>
        int AddTag(Tags tag);

        /// <summary>
        /// Мeтод получения всех тэгов 
        /// </summary>
        /// <returns>Список тэгов</returns>
        List<Tags> GetTags();

        /// <summary>
        /// Мeтод удаления всех тэгов
        /// </summary>
        void RemoveAllTags();

        /// <summary>
        /// Мeтод удаления тэга по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор тэга</param>
        /// <returns>Идентификатор тэга</returns>
        int RemoveTag(int id);

        /// <summary>
        /// Мeтод изменения тэга по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор тэга</param>
        /// <param name="newTag">Измененный тэг</param>
        int UpdateTag(int id, Tags newTag);

        /// <summary>
        /// Метод записи в файл
        /// </summary>
        void WriteToFile();

        /// <summary>
        /// Мeтод получения тэга по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор тэга</param>
        /// <returns>Тэг</returns>
        Tags Get(int id);

    }
}
