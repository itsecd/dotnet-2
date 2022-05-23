using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Lab2Server.Models;
using Telegram.Bot;
using Lab2Server.Services;

namespace Lab2Server
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
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Lab2Server", Version = "v1" });
            });
            services.AddHostedService<TimedHostedService>();
            services.AddHostedService<ReposHostedService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IUserRepository userRepository)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lab2Server v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            string Key = "123";
            string Bot = Configuration.GetValue(Key, "");            
            var botClient = new TelegramBotClient(Bot);
            botClient.StartReceiving(new TelegramUpdateHandler(userRepository));
        }
    }
}
