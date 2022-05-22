using System.Collections.Generic;
using TelegramBot.Model;

namespace TelegramBot.Repository
{
    public interface IUsersRepository
    {
        void ReadFromFile();
        void WriteToFile();
        void AddEventReminder(long id, EventReminder reminder);
        void AddUser(User user);
        List<User> GetUsers();
        void ChangeEventReminder(long userId, long id, EventReminder reminder);
        User FindUser(long id);
        bool IsUserExist(User user);
        void RemoveEventReminder(long userId, long id);
        void RemoveUser(long id);
    }
}