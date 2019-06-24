using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;
using VisitTracker.Interface;

namespace HelloWorld.Grains.Tracker
{
    public class VisitTracker : Grain<VisitTrackerState>, IVisitTracker
    {
        private readonly ILogger<VisitTracker> _logger;

        public VisitTracker(ILogger<VisitTracker> logger)
        {
            _logger = logger;
        }

        public Task<int> GetNumberOfVisits()
        {
            return Task.FromResult(State.NumberOfVisits);
        }

        public async Task VisitAsync()
        {
            var now = DateTime.Now;
            if (!State.FirstVisit.HasValue)
            {
                State.FirstVisit = now;
            }

            State.NumberOfVisits++;
            State.LastVisit = now;
            _logger.LogInformation($"Visit at {now}");
            await WriteStateAsync();
        }
    }
}
