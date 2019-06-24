using System.IO;
using System.Threading.Tasks;
using GranDen.Orleans.Client.CommonLib;
using GranDen.Orleans.Client.CommonLib.TypedOptions;
using HelloWorld.ShareInterface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ConsoleClientDemo
{
    public class CallGrainDemo
    {
        private readonly ILogger<CallGrainDemo> _logger;

        public CallGrainDemo(ILogger<CallGrainDemo> logger)
        {
            _logger = logger;
        }

        public async Task CallHello()
        {
            var (clusterInfo, providerOption) = GetConfigSettings();
            using (var client = OrleansClientBuilder.CreateClient(_logger, clusterInfo, providerOption))
            //using (var client = OrleansClientBuilder.CreateLocalhostClient(_logger, 8310, clusterInfo.ClusterId, clusterInfo.ServiceId))
            {
                await client.ConnectWithRetryAsync();
                _logger.LogInformation("Client successfully connect to silo host");

                var grain = client.GetGrain<IHello>(0);
                _logger.LogInformation("Get greeting grain, start calling RPC method...");

                var returnValue = await grain.SayHello("Hello Orleans");
                _logger.LogInformation($"RPC method return value is \r\n\r\n{{{returnValue}}}\r\n\r\n");

                var return2Value = await grain.SayHello("Hello again");
                _logger.LogInformation($"RPC method return value is \r\n\r\n{{{return2Value}}}\r\n\r\n");

                var return3Value = await grain.SayHello("Hello Orleans");
                _logger.LogInformation($"RPC method return value is \r\n\r\n{{{return3Value}}}\r\n\r\n");

                await client.Close();
                _logger.LogInformation("Client successfully close connection to silo host");
            }
        }

        private static (ClusterInfoOption, OrleansProviderOption) GetConfigSettings()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");

            var configRoot = builder.Build();

            return configRoot.GetSiloSettings();
        }
    }
}