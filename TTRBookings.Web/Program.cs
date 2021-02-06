using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTRBookings.Infrastructure.Data;

namespace TTRBookings.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //describe the 'world' and build it
            var host = CreateHostBuilder(args).Build();

            //ask the 'world' its dependency manager to give us a database context to pass to the repository seed.
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                TTRBookingsContext context = services.GetRequiredService<TTRBookingsContext>();
                try
                {
                    RepositorySeed.SeedDatabase(context);
                }
                catch (Exception e)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError("Error occured during seeding of database, stopping...");
                    logger.LogError(e, e.Message);
                    throw;
                }
            }

            //run the 'world'
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
