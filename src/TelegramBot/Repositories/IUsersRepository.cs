using System.Collections.Generic;
using TelegramBot.Model;

namespace TelegramBot.Repository
{
    public interface IUsersRepository
    {
        void ReadFromFile();
        void WriteToFile();
        void AddEvent(long id, UserEvent userEvent);
        void AddUser(User user);
        List<User> GetUsers();
        void ChangeEvent(long userId, long id, UserEvent userEvent);
        User FindUser(long id);
        bool IsUserExist(User user);
        void RemoveEvent(long userId, long id);
        void RemoveUser(long id);
    }
}