using System.Threading.Tasks;
using TelegramBotServer.Model;

namespace TelegramBotServer.Services
{
    public interface INotificationSenderService
    {
        public Task NotifyAsync(Event someEvent);
    }
}
