using System.Collections.Generic;

namespace Lab2Server.Models
{
    public interface IUserRepository
    {
        List<User> Users { get; }
        void AddUser(string name, int phoneNumber);
        void RemoveUser(int phoneNumber);
        void ChangeName(string name, int phoneNumber);
    }
}