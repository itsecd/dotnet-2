using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TelegramBotServer.Model;

namespace TelegramBotServer.DatabaseContext
{
    public class UsersContext : DbContext
    {
        public DbSet<Subscriber>? Subscribers { get; set; }
        public DbSet<Event>? Events { get; set; }

        public UsersContext(DbContextOptions<UsersContext> options): base(options)
        {
            Database.EnsureCreated();
        }
        
    }
}
