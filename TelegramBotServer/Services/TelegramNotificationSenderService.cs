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
            public int EventId { get; set; }
            public int NewReminder { get; set; }
        }

        private  ITelegramBotClient Bot { get; set; }
        private  ISubscriberRepository SubscriberRepository { get; set; }

        public TelegramNotificationSenderService(ITelegramBotClient bot, ISubscriberRepository subscriberRepository)
        {
            Bot = bot;
            SubscriberRepository = subscriberRepository;
        }

        public Task NotifyAsync(Event someEvent)
        {
            var subs = SubscriberRepository.GetSubscribers();
            var sub = subs?.FirstOrDefault(s => s.EventsId is not null && s.EventsId.Any(eId => eId == someEvent.Id));
            if (sub is not null)
            {
                string message;
                var rest = (int)Math.Round(someEvent.Deadline.Subtract(DateTime.Now).TotalMinutes);

                var inlineKeyboardRows = new List<InlineKeyboardLine> { new InlineKeyboardLine {
                    InlineKeyboardButton.WithCallbackData("Take",
                    JsonSerializer.Serialize(new CallbackData { EventId = someEvent.Id, NewReminder = 0 }))}};

                if (rest > 5)
                    inlineKeyboardRows.Add(new InlineKeyboardLine {
                        InlineKeyboardButton.WithCallbackData("Remind me 5 minutes before the event",
                        JsonSerializer.Serialize(new CallbackData { EventId = someEvent.Id, NewReminder = 5 }))});
                if (rest > 15)
                    inlineKeyboardRows.Add(new InlineKeyboardLine {
                        InlineKeyboardButton.WithCallbackData("Remind me 15 minutes before the event",
                        JsonSerializer.Serialize(new CallbackData { EventId = someEvent.Id, NewReminder = 15 }))});
                if (rest > 30)
                    inlineKeyboardRows.Add(new InlineKeyboardLine {
                        InlineKeyboardButton.WithCallbackData("Remind me 30 minutes before the event",
                        JsonSerializer.Serialize(new CallbackData { EventId = someEvent.Id, NewReminder = 30 }))});
                if (rest > 60)
                    inlineKeyboardRows.Add(new InlineKeyboardLine {
                        InlineKeyboardButton.WithCallbackData("Remind me 60 minutes before the event",
                        JsonSerializer.Serialize(new CallbackData { EventId = someEvent.Id, NewReminder = 60 }))});

                if (rest < 0)
                    message = $"Your event {someEvent.Id} overdue is {Math.Abs(rest)} minutes";
                else
                    message = $"Your event {someEvent.Id} will happen in {rest} minutes";


                InlineKeyboardMarkup inlineKeyboard = new(inlineKeyboardRows);
                return Bot.SendTextMessageAsync(chatId: sub.ChatId,
                                                     text: message,
                                                     replyMarkup: inlineKeyboard);
            }
            else
                return Task.CompletedTask;
        }
    }
}
