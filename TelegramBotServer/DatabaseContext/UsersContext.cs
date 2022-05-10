using Microsoft.EntityFrameworkCore;
using TelegramBotServer.Model;

namespace TelegramBotServer.DatabaseContext
{
    public class UsersContext : DbContext
    {
        public DbSet<Subscriber>? Subscribers { get; set; }
        public DbSet<Event>? Events { get; set; }

        public UsersContext(DbContextOptions<UsersContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

    }
}
