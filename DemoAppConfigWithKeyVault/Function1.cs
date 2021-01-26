using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DemoAppConfigWithKeyVault
{
    public class Function1
    {
        private readonly IApplicationConfigService _applicationConfigService;

        public Function1(IApplicationConfigService applicationConfigService)
        {
            _applicationConfigService = applicationConfigService;
        }

        [FunctionName("ConfigSettings")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var jsonText = JsonConvert.SerializeObject(_applicationConfigService.GetConfig);

            return new OkObjectResult(jsonText);
        }
    }
}
