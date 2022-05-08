using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelegramBotServer.DatabaseContext;
using TelegramBotServer.Model;

namespace TelegramBotServer.Repository
{
    public class DbEventRepository : IEventRepository
    {
        private IServiceProvider _scopeFactory;
        private ILogger<DbEventRepository> _logger;
        public DbEventRepository(IServiceProvider scopeFactory, ILogger<DbEventRepository> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public int AddEvent(Event newEvent)
        {
            var dbContext = _scopeFactory.CreateScope()
                .ServiceProvider.GetRequiredService<UsersContext>();

            dbContext.Events.Add(newEvent);
            dbContext.SaveChanges();

            _logger.LogInformation($"Add Event with id {newEvent.Id}");

            return newEvent.Id;
        }

        public void ChangeEvent(int id, Event newEvent)
        {
            var dbContext = _scopeFactory.CreateScope()
                .ServiceProvider.GetRequiredService<UsersContext>();

            var chEvent = dbContext.Events.FirstOrDefault(e => e.Id == id);

            if (chEvent is null)
                return;

            dbContext.Update(chEvent).CurrentValues.SetValues(newEvent);
            dbContext.SaveChanges();
        }

        public Event GetEvent(int id)
        {
            var dbContext = _scopeFactory.CreateScope()
                .ServiceProvider.GetRequiredService<UsersContext>();

            return dbContext.Events.Find(id);
        }

        public IEnumerable<Event> GetEvents()
        {
            var dbContext = _scopeFactory.CreateScope()
                .ServiceProvider.GetRequiredService<UsersContext>();

            return dbContext.Events;
        }

        public bool RemoveEvent(int id)
        {
            var dbContext = _scopeFactory.CreateScope()
                .ServiceProvider.GetRequiredService<UsersContext>();

            var delEvent = dbContext.Events.FirstOrDefault(e => e.Id == id);

            if (delEvent is null)
                return false;

            dbContext.Events.Remove(delEvent);
            dbContext.SaveChanges();

            _logger.LogInformation($"Remove Event with id {delEvent.Id}");

            return true;
        }
    }
}
