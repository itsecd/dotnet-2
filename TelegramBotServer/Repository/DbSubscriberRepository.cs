using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelegramBotServer.DatabaseContext;
using TelegramBotServer.Model;
using TelegramBotServer.Validators;

namespace TelegramBotServer.Repository
{
    public class DbSubscriberRepository : ISubscriberRepository
    {

        private IServiceProvider _scopeFactory;
        private ILogger<DbSubscriberRepository> _logger;
        public DbSubscriberRepository(IServiceProvider scopeFactory, ILogger<DbSubscriberRepository> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }
        public int AddSubscriber(Subscriber newSub)
        {
            var dbContext = _scopeFactory.CreateScope()
                .ServiceProvider.GetRequiredService<UsersContext>();

            var validator = new SubscriberValidator(dbContext.Events);
            if (!validator.Validate(newSub))
                throw new ArgumentException("Invalid subscriber");

            dbContext.Subscribers.Add(newSub);
            dbContext.SaveChanges();

            _logger.LogInformation($"Add Subscriber with id {newSub.Id}");

            return newSub.Id;
        }

        public bool ChangeSubscriber(int id, Subscriber newSub)
        {
            var dbContext = _scopeFactory.CreateScope()
                .ServiceProvider.GetRequiredService<UsersContext>();

            var validator = new SubscriberValidator(dbContext.Events);
            if (!validator.Validate(newSub))
                throw new ArgumentException("Invalid subscriber");

            var chSub = dbContext.Subscribers.FirstOrDefault(s => s.Id == id);
            if (chSub is null)
                return false;

            dbContext.Entry(chSub).CurrentValues.SetValues(newSub);
            dbContext.SaveChanges();

            _logger.LogInformation($"Subscriber with id {id} was changed");

            return true;
        }

        public Subscriber GetSubscriber(int id)
        {
            var dbContext = _scopeFactory.CreateScope()
                .ServiceProvider.GetRequiredService<UsersContext>();

            return dbContext.Subscribers.Find(id);
        }

        public IEnumerable<Subscriber> GetSubscribers()
        {
            var dbContext = _scopeFactory.CreateScope()
               .ServiceProvider.GetRequiredService<UsersContext>();

            return dbContext.Subscribers;
        }

        public bool RemoveSubscriber(int id)
        {
            var dbContext = _scopeFactory.CreateScope()
               .ServiceProvider.GetRequiredService<UsersContext>();

            var delSub = dbContext.Subscribers.Find(id);
            if (delSub is null)
                return false;

            dbContext.Subscribers.Remove(delSub);
            dbContext.SaveChanges();

            _logger.LogInformation($"Subscriber with id {id} was changed");
            return true;
        }
    }
}
