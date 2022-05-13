using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using TelegramBotServer.DatabaseContext;
using TelegramBotServer.Model;
using TelegramBotServer.Validators;

namespace TelegramBotServer.Repository
{
    public class DbEventRepository : IEventRepository
    {
        readonly private IServiceProvider _scopeFactory;
        readonly private ILogger<DbEventRepository> _logger;
        public DbEventRepository(IServiceProvider scopeFactory, ILogger<DbEventRepository> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public int AddEvent(Event newEvent)
        {
            var dbContext = _scopeFactory.CreateScope()
                .ServiceProvider.GetRequiredService<UsersContext>();
            if (dbContext.Events is null)
                throw new Exception("Database context is null!");
            else
            {
                var validator = new EventValidator();
                if (!validator.Validate(newEvent))
                    throw new ArgumentException("Invalid event");

                dbContext.Events.Add(newEvent);
                dbContext.SaveChanges();

                _logger.LogInformation($"Add Event with id {newEvent.Id}");
                return newEvent.Id;
            }
        }

        public bool ChangeEvent(int id, Event newEvent)
        {
            var dbContext = _scopeFactory.CreateScope()
                .ServiceProvider.GetRequiredService<UsersContext>();
            if (dbContext.Events is null)
                throw new Exception("Database context is null!");
            else
            {
                var validator = new EventValidator();
                if (!validator.Validate(newEvent))
                    throw new ArgumentException("Invalid event");

                var chEvent = dbContext.Events.FirstOrDefault(e => e.Id == id);

                if (chEvent is null)
                    return false;

                dbContext.Update(chEvent).CurrentValues.SetValues(newEvent);
                dbContext.SaveChanges();
                return true;
            }
        }

        public Event? GetEvent(int id)
        {
            var dbContext = _scopeFactory.CreateScope()
                .ServiceProvider.GetRequiredService<UsersContext>();

            return dbContext.Events?.Find(id);
        }

        public IEnumerable<Event>? GetEvents()
        {
            var dbContext = _scopeFactory.CreateScope()
                .ServiceProvider.GetRequiredService<UsersContext>();

            return dbContext.Events;
        }

        public bool RemoveEvent(int id)
        {
            var dbContext = _scopeFactory.CreateScope()
                .ServiceProvider.GetRequiredService<UsersContext>();
            if (dbContext.Events is null)
                throw new Exception("Database context is null!");
            else
            {
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
}
