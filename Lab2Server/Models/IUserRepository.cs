namespace Lab2Server.Models
{
    public interface IUserRepository
    {
        void AddNewUser(User newUser);
        void AddReminder(int userId, Reminder reminder);
        void ChangeName(int userId, string newName);
        void ChangeReminder(int userId, int id, Reminder reminder);
        bool ExistUser(User user);
        User FindUser(int id);
        void RemoveReminder(int userId, int id);
        void RemoveUser(int userId);
    }
}