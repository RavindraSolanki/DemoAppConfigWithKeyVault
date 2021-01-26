using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

namespace DemoAppConfigWithKeyVault
{
    public interface IApplicationConfigService
    {
        ApplicationConfig GetConfig { get; }
    }
    public class ApplicationConfigService : IApplicationConfigService
    {
        private readonly IConfiguration _config;
        private readonly IConfigurationRefresher _configRefresher;

        public ApplicationConfigService(IConfiguration configuration, IConfigurationRefresherProvider refresherProvider)
        {
            _config = configuration;
            _configRefresher = refresherProvider.Refreshers.First();
        }

        public ApplicationConfig GetConfig
        {
            get
            {
                _configRefresher.TryRefreshAsync();
                var configSection = _config.GetSection("DemoAppConfigWithKeyVault");
                return configSection.Get<ApplicationConfig>();
            }
        }
    }

    public class ApplicationConfig
    {
        public string KeyOne { get; set; }
        public string KeyTwo { get; set; }
        public string KeyThree { get; set; }
        public string Sentinel { get; set; }
    }
}
