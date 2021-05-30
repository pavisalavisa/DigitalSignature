using System;
using System.Threading.Tasks;
using Application.Common.Contracts;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

#pragma warning disable 1591

namespace Api
{
    // Using top level statements breaks EF Core migrations tool because
    // it has convention based DbContext lookup
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            await MigrateAndSeedDatabase(host);

            await host.RunAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

        static async Task MigrateAndSeedDatabase(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<DigitalSignatureManagerDbContext>();

                await context.Database.MigrateAsync();

                var roleManager = services.GetRequiredService<IApplicationRoleManager>();
                var userManager = services.GetRequiredService<IApplicationUserManager>();

                await DigitalSignatureManagerDbContextSeed.SeedSampleDataAsync(context, roleManager, userManager);
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                logger.LogError(ex, "An error occurred while migrating or seeding the database.");

                throw;
            }
        }
    }
}