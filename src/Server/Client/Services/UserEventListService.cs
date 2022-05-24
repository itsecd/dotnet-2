using System;
using System.Collections.Generic;
using DynamicData;
using Server.Model;

namespace Client.Services
{
    public sealed class UserEventListService
    {
        private readonly SourceCache<UserEvent, int> _userEvents = new(uEvent => uEvent.Id);

        public IObservable<IChangeSet<UserEvent, int>> Connect()
        {
            return _userEvents.Connect();
        }

        public void Update(List<UserEvent> userEvents)
        {
            _userEvents.Clear();
            _userEvents.AddOrUpdate(userEvents);
        }

        public void Add(UserEvent userEvent)
        {
            _userEvents.AddOrUpdate(userEvent);
        }

        public void Remove(UserEvent userEvent)
        { 
            _userEvents.Remove(userEvent);
        }

    }
}
