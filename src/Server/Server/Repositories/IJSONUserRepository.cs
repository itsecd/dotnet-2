using Server.Model;
using System.Collections.Generic;

namespace Server.Repositories
{
    public interface IJSONUserRepository
    {
        public List<User> Users { get; }
        public User GetUser(int id);
        public IEnumerable<User> GetUsers();
        public void AddUser(User element);
        public void UpdateUser(int id, User element);
        public void DeleteUsers(int id);
        public void DeleteAllUsers();
        public void SaveData();
        public void LoadData();
    }
}
