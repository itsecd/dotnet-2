using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;


namespace Server
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
                                          Host.CreateDefaultBuilder(args)
                                              .ConfigureWebHostDefaults(webBuilder =>
                                              {
                                                  webBuilder.UseStartup<Startup>();
                                              });
        /*                     Host.CreateDefaultBuilder(args)
                                 .ConfigureWebHostDefaults(webBuilder =>
                                 {
                                     webBuilder
                                      .ConfigureKestrel(options =>
                                      {
                                          options.ListenLocalhost(7050);
                                      });
                                     webBuilder.UseStartup<Startup>();
                                 });*/

    }
}
