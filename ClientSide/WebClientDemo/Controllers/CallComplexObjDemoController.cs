using System;
using System.Threading.Tasks;
using ComplexTypeDemo.ShareInterface;
using GranDen.Orleans.Client.CommonLib;
using GranDen.Orleans.Client.CommonLib.TypedOptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WebClientDemo.Models;

namespace WebClientDemo.Controllers
{
    public class CallComplexObjDemoController : Controller
    {
        private const string DataKey = "call_result";

        private readonly ILogger<CallComplexObjDemoController> _logger;
        private readonly ClusterInfoOption _clusterInfo;
        private readonly OrleansProviderOption _providerOption;

        public CallComplexObjDemoController(ILogger<CallComplexObjDemoController> logger, IOptionsMonitor<ClusterInfoOption> clusterInfoOptionsMonitor,
            IOptionsMonitor<OrleansProviderOption> providerOptionsMonitor)
        {
            _logger = logger;
            _clusterInfo = clusterInfoOptionsMonitor.CurrentValue;
            _providerOption = providerOptionsMonitor.CurrentValue;
        }

        public IActionResult Index()
        {
            if (!TempData.TryGetValue(DataKey, out var o))
            {
                return View();
            }
            var callResultViewModel = JsonConvert.DeserializeObject<CallComplexObjViewModel>((string)o);

            return View(callResultViewModel);
        }

        public async Task<IActionResult> CallGrainRpcDemo()
        {
            using (var client = OrleansClientBuilder.CreateClient(_logger, _clusterInfo, _providerOption))
            {
                await client.ConnectWithRetryAsync();
                _logger.LogInformation("Client successfully connect to silo host");

                var grain = client.GetGrain<ICreateRecord>(0);
                _logger.LogInformation("Get create record grain, start calling RPC method...");

                var retValue = await grain.CreateRandomData();
                _logger.LogInformation("RPC method returns: {RetValue}", retValue);

                await client.Close();
                _logger.LogInformation("Client successfully close connection to silo host");

                TempData[DataKey] = JsonConvert.SerializeObject(new CallComplexObjViewModel { Result = retValue });

                return RedirectToAction(nameof(Index));
            }
        }
    }
}