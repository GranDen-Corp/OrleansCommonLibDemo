using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GranDen.Orleans.Client.CommonLib;
using HelloWorld.ShareInterface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebClientDemo.Models;

namespace WebClientDemo.Controllers
{
    public class HomeController : Controller
    {
        private const string DataKey = "call_result";

        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
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

        public async Task<IActionResult> CallGrainDemo()
        {
            var (clusterInfo, providerOption) = _configuration.GetSection("Orleans").GetSiloSettings();

            using (var client = OrleansClientBuilder.CreateClient(_logger, clusterInfo, providerOption, new[] { typeof(IHello) }))
            {
                await client.ConnectWithRetryAsync();
                _logger.LogInformation("Client successfully connect to silo host");

                var grain = client.GetGrain<IHello>(0);
                _logger.LogInformation("Get greeting grain, start calling RPC method...");

                var returnValue = await grain.SayHello("Hello Orleans");
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
