using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<EventWatcherHostedService>();

            services.AddHttpClient("telegram")
               .AddTypedClient<ITelegramBotClient>(httpClient
                   => new TelegramBotClient(Configuration["BotToken"], httpClient));

            services.AddDbContext<UsersContext>(options =>
               options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<IStartupTask, SetWebHookTask>();
            services.AddTransient<CommandHandlerService>();
            services.AddSingleton<IEventRepository, DbEventRepository>();
            services.AddSingleton<ISubscriberRepository, DbSubscriberRepository>();
            services.AddTransient<INotificationSenderService, TelegramNotificationSenderService>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TelegramBotServer", Version = "v1" });
            });
            services.AddControllers().AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TelegramBotServer v1"));
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            //app.UseAuthorization();

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
