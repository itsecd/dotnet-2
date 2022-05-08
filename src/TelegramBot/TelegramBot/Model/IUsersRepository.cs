namespace TelegramBot.Model
{
    public interface IUsersRepository
    {
        void AddEventReminder(long id, EventReminder reminder);
        void AddUser(User user);
        void ChangeEventReminder(long userId, long id, EventReminder reminder);
        User FindUser(long id);
        bool IsUserExist(User user);
        void RemoveEventReminder(long userId, long id);
        void RemoveUser(long id);
    }
}