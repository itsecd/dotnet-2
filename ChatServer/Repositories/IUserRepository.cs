using System.Threading.Tasks;

namespace ChatServer.Repositories
{
    public interface IUserRepository
    {
        void AddUser(string nameUser);
        Task ReadAsyncToFile();
        Task WriteAsyncToFile();

        public bool IsUserExist(string userName);
    }
}