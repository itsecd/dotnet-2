using System;
using System.Collections.Generic;
using DynamicData;
using Lab2Server.Models;

namespace BotClient.Services
{
    public sealed class UserEventListService
    {
        private readonly SourceCache<Reminder, int> _userEvents = new(uEvent => uEvent.Id);

        public IObservable<IChangeSet<Reminder, int>> Connect()
        {
            return _userEvents.Connect();
        }

        public void Update(List<Reminder> userEvents)
        {
            _userEvents.Clear();
            _userEvents.AddOrUpdate(userEvents);
        }

        public void Add(Reminder userEvent)
        {
            _userEvents.AddOrUpdate(userEvent);
        }

        public void Remove(Reminder userEvent)
        {
            _userEvents.Remove(userEvent);
        }

    }
}