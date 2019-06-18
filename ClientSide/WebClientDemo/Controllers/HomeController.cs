using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GranDen.Orleans.Client.CommonLib;
using GranDen.Orleans.Client.CommonLib.TypedOptions;
using HelloWorld.ShareInterface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WebClientDemo.Models;

namespace WebClientDemo.Controllers
{
    public class HomeController : Controller
    {
        private const string DataKey = "call_result";

        private readonly ILogger<HomeController> _logger;
        private readonly ClusterInfoOption _clusterInfo;
        private readonly OrleansProviderOption _providerOption;

        public HomeController(ILogger<HomeController> logger, 
            IOptionsMonitor<ClusterInfoOption> clusterInfoOptionsMonitor, IOptionsMonitor<OrleansProviderOption> providerOptionsMonitor)
        {
            _logger = logger;
            _clusterInfo = clusterInfoOptionsMonitor.CurrentValue;
            _providerOption = providerOptionsMonitor.CurrentValue;
        }

        public IActionResult Index()
        {
            if (!TempData.TryGetValue(DataKey, out Object o))
            {
                return View();
            }
            var callResultViewModel = JsonConvert.DeserializeObject<ResultViewModel>((string)o);

            return View(callResultViewModel);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> CallGrainDemo(string input)
        {
            using (var client = OrleansClientBuilder.CreateClient(_logger, _clusterInfo, _providerOption, new[] { typeof(IHello) }))
            {
                await client.ConnectWithRetryAsync();
                _logger.LogInformation("Client successfully connect to silo host");

                var grain = client.GetGrain<IHello>(0);
                _logger.LogInformation("Get greeting grain, start calling RPC method...");

                var returnValue = await grain.SayHello(input);
                _logger.LogInformation($"RPC method return value is \r\n\r\n{{{returnValue}}}\r\n\r\n");

                await client.Close();
                _logger.LogInformation("Client successfully close connection to silo host");

                TempData[DataKey] = JsonConvert.SerializeObject(new ResultViewModel { Result = returnValue });

                return RedirectToAction(nameof(Index));
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
