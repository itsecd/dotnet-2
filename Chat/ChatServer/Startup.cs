using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChatServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // ConfigureServices - ������������ ��� ���������� � �������� ����������� ������ DI ����������
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(); // not used
            services.AddSignalR();
        }

        // Configure - ����� ��� ������ ���������� ����� ������
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting(); // not used

            app.UseAuthorization(); // not used

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chatroom");
            });
        }
    }
}
