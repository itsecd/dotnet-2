using System.Threading.Tasks;

namespace ChatServer.Repositories
{
    public interface IUserRepository
    {
        void AddUser(string nameUser);
        Task ReadAsync();
        Task WriteAsync();

        public bool IsUserExist(string userName);
    }
}