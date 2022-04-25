using System.Threading.Tasks;

namespace TelegramBotServer
{
    public interface IStartupTask
    {
        Task Execute();
    }
}
