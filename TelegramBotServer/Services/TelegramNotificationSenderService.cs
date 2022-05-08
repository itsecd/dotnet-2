using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotServer.Model;
using TelegramBotServer.Repository;
using InlineKeyboardLine = System.Collections.Generic.List<Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton>;

namespace TelegramBotServer.Services
{
    public class TelegramNotificationSenderService : INotificationSenderService
    {
        public class CallbackData
        {
            public int eventId { get; set; }
            public int newReminder { get; set; }
        }

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
                string message;
                var rest = (int)Math.Round(someEvent.Deadline.Subtract(DateTime.Now).TotalMinutes);

                var inlineKeyboardRows = new List<InlineKeyboardLine> { new InlineKeyboardLine { 
                    InlineKeyboardButton.WithCallbackData("Take",
                    JsonSerializer.Serialize(new CallbackData { eventId = someEvent.Id, newReminder = 0 }))}};

                if (rest > 5)
                    inlineKeyboardRows.Add(new InlineKeyboardLine {
                        InlineKeyboardButton.WithCallbackData("Remind me 5 minutes before the event",
                        JsonSerializer.Serialize(new CallbackData { eventId = someEvent.Id, newReminder = 5 }))});
                if (rest > 15)
                    inlineKeyboardRows.Add(new InlineKeyboardLine { 
                        InlineKeyboardButton.WithCallbackData("Remind me 15 minutes before the event",
                        JsonSerializer.Serialize(new CallbackData { eventId = someEvent.Id, newReminder = 15 }))});
                if (rest > 30)
                    inlineKeyboardRows.Add(new InlineKeyboardLine { 
                        InlineKeyboardButton.WithCallbackData("Remind me 30 minutes before the event",
                        JsonSerializer.Serialize(new CallbackData { eventId = someEvent.Id, newReminder = 30 }))});
                if (rest > 60)
                    inlineKeyboardRows.Add(new InlineKeyboardLine { 
                        InlineKeyboardButton.WithCallbackData("Remind me 60 minutes before the event",
                        JsonSerializer.Serialize(new CallbackData { eventId = someEvent.Id, newReminder = 60 }))});

                if (rest < 0)
                    message = $"Your event {someEvent.Id} overdui is {Math.Abs(rest)} minutes";
                else
                    message = $"Your event {someEvent.Id} will happen in {rest} minutes";


                InlineKeyboardMarkup inlineKeyboard = new(inlineKeyboardRows);
                return _bot.SendTextMessageAsync(chatId: sub.ChatId,
                                                     text: message,
                                                     replyMarkup: inlineKeyboard);
            }
            else
                return Task.CompletedTask;
        }
    }
}
