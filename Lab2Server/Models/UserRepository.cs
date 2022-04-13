using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2Server.Models
{
    public class UserRepository : IUserRepository
    {
        public List<User> Users { get; set; } = new List<User>();

        public void AddUser(string name, int phoneNumber)
        {
            User newUser = new User(name, phoneNumber);
        }
        public void RemoveUser(int phoneNumber)
        {

        }
        public void ChangeName(string name, int phoneNumber)
        {

        }
    }
}
