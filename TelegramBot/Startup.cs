using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using TelegramBot.Services;
using TelegramBot.Repository;
using Microsoft.Extensions.Configuration;

namespace TelegramBot
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public string TelegramBotKey { get; init; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            TelegramBotKey = Configuration.GetValue<string>("TelegramBotKey");
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IUsersRepository, UsersRepository>();
            services.AddHostedService<RepositoryHostedService>();
            services.AddHostedService<TimedHostedService>();
            services.AddGrpc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IUsersRepository usersRepository)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<TgEventService>();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
            var botClient = new TelegramBotClient(TelegramBotKey);

            botClient.StartReceiving(new TelegramBotUpdateHandler(usersRepository));
        }
    }
}
