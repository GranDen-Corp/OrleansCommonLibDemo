using HelloWorld.ShareInterface;
using Microsoft.Extensions.Logging;
using Orleans;
using System;
using System.Threading.Tasks;

namespace HelloWorld.Grains.VisitTrackerGrain
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

    public class VisitTrackerState
    {
        public DateTime? FirstVisit { get; set; }
        public DateTime? LastVisit { get; set; }
        public int NumberOfVisits { get; set; }
    }
}
