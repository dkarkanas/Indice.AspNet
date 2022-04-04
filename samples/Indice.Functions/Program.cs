﻿using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Indice.Functions
{
    public class Program
    {
        public async static Task Main(params string[] args) {
            // Create the host builder instance.
            var host = new HostBuilder();
            // Configure function host.
            host.ConfigureAppConfiguration((context, config) => {
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                      .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
            })
            .ConfigureFunctionsWorkerDefaults(builder => { })
            .ConfigureServices(services => { })
            .ConfigureCampaigns((configuration, options) => {
                options.ConfigureDbContext = builder => builder.UseSqlServer(configuration.GetConnectionString("CampaignsDb"));
                options.DatabaseSchema = "cmp";
                options.UseEventDispatcherAzure();
                //options.UsePushNotificationServiceAzure();
                options.UseIdentityContactResolver(resolverOptions => {
                    resolverOptions.BaseAddress = new Uri(configuration["IdentityServer:BaseAddress"]);
                    resolverOptions.ClientId = configuration["IdentityServer:ClientId"];
                    resolverOptions.ClientSecret = configuration["IdentityServer:ClientSecret"];
                });
            })
            .UseEnvironment(Environment.GetEnvironmentVariable("ENVIRONMENT"));
            // Build host and run.
            await host.Build().RunAsync();
        }
    }
}
