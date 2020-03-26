using System;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DatingApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // km - a host is an object that encapsulates dependency injection, logging, configuration, IHostedService
            var host = CreateHostBuilder(args).Build();

            // km to load the userseeddata from the seed file
            using(var scope = host.Services.CreateScope())
            {
                // km scope and services are neeeded to get the services - like dependency injection.
                var services  = scope.ServiceProvider;
                try{
                    var context = services.GetRequiredService<DataContext>();
                    // do any pending database migrations
                    context.Database.Migrate();
                    // seed the users by passing the datacontext
                    Seed.SeedUsers(context);

                }catch(Exception ex){
                    // now we are getting logger service
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occured during migration");           
                }
            }

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
