using Server.Model;
using System.Collections.Generic;

namespace Server.Repositories
{
    public interface IJSONUserEventRepository
    {
        public List<UserEvent> UserEvents { get; }
        public UserEvent GetUserEvent(int id);
        public IEnumerable<UserEvent> GetUserEvents();
        public void AddUserEvent(UserEvent element);
        public void UpdateUserEvent(int id, UserEvent element);
        public void DeleteUserEvent(int id);
        public void DeleteAllUserEvents();
        public void SaveData();
        public void LoadData();
    }
}
