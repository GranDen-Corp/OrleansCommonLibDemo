using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GranDen.Orleans.Client.CommonLib;
using GranDen.Orleans.Client.CommonLib.TypedOptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyReminder.ShareInterface;
using Newtonsoft.Json;
using WebClientDemo.Models;

namespace WebClientDemo.Controllers
{
    public class LongTaskDemoController : Controller
    {
        private readonly ILogger<LongTaskDemoController> _logger;
        private readonly ClusterInfoOption _clusterInfo;
        private readonly OrleansProviderOption _providerOption;
        private const string tempDataKey = @"CallMyReminderGrainResult";

        public LongTaskDemoController(ILogger<LongTaskDemoController> logger,
            IOptionsMonitor<ClusterInfoOption> clusterInfoOptionsMonitor, IOptionsMonitor<OrleansProviderOption> providerOptionsMonitor)
        {
            _logger = logger;
            _clusterInfo = clusterInfoOptionsMonitor.CurrentValue;
            _providerOption = providerOptionsMonitor.CurrentValue;
        }

        public IActionResult Index(NUlid.Ulid? runSessionId)
        {
            if (!runSessionId.HasValue)
            {
                return View();
            }

            ViewData["signalrHubUrl"] = @"/running_status";
            ViewData["sessionId"] = runSessionId;

            if(TempData.ContainsKey(tempDataKey))
            {
                var viewModel = JsonConvert.DeserializeObject<LongTaskViewModel>(TempData[tempDataKey].ToString());
                return View(viewModel);
            }

            return View();
        }

        public async Task<IActionResult> CallGrainAlarm()
        {
            using (var client =
                OrleansClientBuilder.CreateClient(_logger, _clusterInfo, _providerOption))
            {
                await client.ConnectWithRetryAsync();

                var runSessionId = NUlid.Ulid.NewUlid();
                var grainGuid = runSessionId.ToGuid();

                var demoGrain = client.GetGrain<IMyReminder>(grainGuid);
                var callResult = await demoGrain.Alarm();
                await client.Close();

                TempData[tempDataKey] = JsonConvert.SerializeObject(new LongTaskViewModel { Result = callResult, RunSessionId = runSessionId });

                return RedirectToAction(nameof(Index), new { runSessionId });
            }
        }
    }
}