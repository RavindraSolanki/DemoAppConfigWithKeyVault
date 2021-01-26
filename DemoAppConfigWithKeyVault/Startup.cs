using System;
using System.IO;
using Azure.Identity;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(DemoAppConfigWithKeyVault.Startup))]

namespace DemoAppConfigWithKeyVault
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddAzureAppConfiguration();
            builder.Services.AddScoped<IApplicationConfigService, ApplicationConfigService>();
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            var azureConfigConnectionString = "<add-app-configuration-connection-string-here>";

            builder.ConfigurationBuilder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            builder.ConfigurationBuilder.AddAzureAppConfiguration(options =>
            {
                options
                    .Connect(azureConfigConnectionString)
                    .Select(keyFilter: "DemoAppConfigWithKeyVault:*")
                    .ConfigureRefresh(refreshOptions =>
                    {
                        refreshOptions.Register(key: "DemoAppConfigWithKeyVault:Sentinel", refreshAll: true);
                        refreshOptions.SetCacheExpiration(TimeSpan.FromMinutes(1));
                    });
                
                options.ConfigureKeyVault(keyVaultOptions =>
                {
                    keyVaultOptions.SetCredential(new DefaultAzureCredential());
                });
            });
        }
    }
}
