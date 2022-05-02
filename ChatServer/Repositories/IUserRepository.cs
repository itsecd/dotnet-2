using System.Threading.Tasks;

namespace ChatServer.Repositories
{
    public interface IUserRepository
    {
        void AddUser(string nameUser);
        Task ReadFromFileAsync();
        Task WriteAsyncToFile();

        public bool IsUserExist(string userName);
    }
}