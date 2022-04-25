using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using TelegramBotServer.Model;
using TelegramBotServer.Repository;

namespace TelegramBotServer.Services
{
    public class TelegramNotificationSenderService : INotificationSenderService
    {
        public ITelegramBotClient _bot { get; private set; }
        public ISubscriberRepository _subscriberRepository { get; private set; }

        public TelegramNotificationSenderService(ITelegramBotClient bot, ISubscriberRepository subscriberRepository)
        {
            _bot = bot;
            _subscriberRepository = subscriberRepository;
        }

        public Task NotifyAsync(Event someEvent)
        {
            var subs = _subscriberRepository.GetSubscribers();
            var sub = subs.FirstOrDefault(s => s.EventsId.Any(eId => eId == someEvent.Id));
            if (sub is not null)
            {
                var rest = someEvent.Deadline.Subtract(DateTime.Now);
                return _bot.SendTextMessageAsync(chatId: sub.ChatId, text: $"Your event {someEvent.Id} will happen in {rest} minutes");
            }
            else
                return Task.CompletedTask;
        }
    }
}
