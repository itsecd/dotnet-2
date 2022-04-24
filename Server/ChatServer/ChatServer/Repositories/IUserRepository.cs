using System.Threading.Tasks;

namespace ChatServer.Repositories
{
    public interface IUserRepository
    {
        void AddUser(string nameUser);
        Task ReadAsync();
        void ReadFile();
        Task WriteAsync();
        void WriteToFile();

        public bool IsUserExist(string userName);
    }
}