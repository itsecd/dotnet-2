using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotServer.Enums;
using TelegramBotServer.Model;
using TelegramBotServer.Repository;
using InlineKeyboardLine = System.Collections.Generic.List<Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton>;

namespace TelegramBotServer.Services
{
    public class TelegramNotificationSenderService : INotificationSenderService
    {

        private ITelegramBotClient _bot;
        private ISubscriberRepository _subscriberRepository;
        private SubscriberSessions _sessions;

        public TelegramNotificationSenderService(
            ITelegramBotClient bot,
            ISubscriberRepository subscriberRepository,
            SubscriberSessions sessions)
        {
            _bot = bot;
            _subscriberRepository = subscriberRepository;
            _sessions = sessions;
        }

        public async Task NotifyAsync(Event someEvent)
        {
            var subs = _subscriberRepository.GetSubscribers();
            var sub = subs?.FirstOrDefault(s => s.EventsId is not null && s.EventsId.Any(eId => eId == someEvent.Id));
            if (sub is not null)
            {
                string message;
                var rest = (int)Math.Round(someEvent.Deadline.Subtract(DateTime.Now).TotalMinutes);

                var inlineKeyboardRows = new List<InlineKeyboardLine> { new InlineKeyboardLine {
                    InlineKeyboardButton.WithCallbackData("Take",
                    JsonSerializer.Serialize(new CallbackData { Type= CallbackDataType.Notification, Data = $"{0}" }))}};

                if (rest > 5)
                    inlineKeyboardRows.Add(new InlineKeyboardLine {
                        InlineKeyboardButton.WithCallbackData("Remind me 5 minutes before the event",
                        JsonSerializer.Serialize(new CallbackData { Type= CallbackDataType.Notification, Data = $"{5}" }))});
                if (rest > 15)
                    inlineKeyboardRows.Add(new InlineKeyboardLine {
                        InlineKeyboardButton.WithCallbackData("Remind me 15 minutes before the event",
                        JsonSerializer.Serialize(new CallbackData { Type= CallbackDataType.Notification, Data = $"{15}" }))});
                if (rest > 30)
                    inlineKeyboardRows.Add(new InlineKeyboardLine {
                        InlineKeyboardButton.WithCallbackData("Remind me 30 minutes before the event",
                        JsonSerializer.Serialize(new CallbackData { Type= CallbackDataType.Notification, Data = $"{30}" }))});
                if (rest > 60)
                    inlineKeyboardRows.Add(new InlineKeyboardLine {
                        InlineKeyboardButton.WithCallbackData("Remind me 60 minutes before the event",
                        JsonSerializer.Serialize(new CallbackData { Type= CallbackDataType.Notification, Data = $"{60}" }))});

                if (rest < 0)
                    message = $"Your event {someEvent.Id} overdue is {Math.Abs(rest)} minutes";
                else
                    message = $"Your event {someEvent.Id} will happen in {rest} minutes";

                InlineKeyboardMarkup inlineKeyboard = new(inlineKeyboardRows);
                var messageId = (await _bot.SendTextMessageAsync(chatId: sub.ChatId,
                                                     text: message,
                                                     replyMarkup: inlineKeyboard)).MessageId;
                if (_sessions.Sessions.ContainsKey(sub.ChatId))
                {
                    _sessions.Sessions[sub.ChatId].NotificatedEventId.Add(messageId, someEvent.Id);
                    _sessions.Sessions[sub.ChatId].CreationTime = DateTime.Now;
                }
                else
                {
                    _sessions[sub.ChatId] = new SubscriberSession
                    {
                        CreationTime = DateTime.Now,
                        NotificatedEventId = new Dictionary<int, int> { { messageId, someEvent.Id } }
                    };
                }

                return;
            }
            else
                return;
        }
    }
}
