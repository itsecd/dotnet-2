using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using Telegram.Bot;
using TelegramBotServer.DatabaseContext;
using TelegramBotServer.Repository;
using TelegramBotServer.Services;

namespace TelegramBotServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<EventWatcherHostedService>();
            services.AddHostedService<SetWebHookService>();

            services.AddHttpClient("telegram")
               .AddTypedClient<ITelegramBotClient>(httpClient
                   => new TelegramBotClient(Configuration["BotToken"], httpClient));

            services.AddDbContext<UsersContext>(options =>
               options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<CommandHandlerService>();
            services.AddSingleton<IEventRepository, DbEventRepository>();
            services.AddSingleton<ISubscriberRepository, DbSubscriberRepository>();
            services.AddSingleton<PlanSessions>(); // если пользователь не контактирует с ботом >20 минут - подчищать сессию (будет бегать сервис по крону) 
            services.AddTransient<INotificationSenderService, TelegramNotificationSenderService>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TelegramBotServer", Version = "v1" });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
            services.AddControllers().AddNewtonsoftJson();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TelegramBotServer v1"));
            }

            app.UseRouting();

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "webhook",
                    pattern: $"bot/{Configuration["BotToken"]}",
                    new { controller = "Webhook", action = "Post" });

                endpoints.MapControllers();
            });
        }
    }
}
