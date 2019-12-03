using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans.Runtime;
using VisitTracker.ShareInterface;

namespace HelloWorld.Grain.Tracker
{
    public class VisitTracker : Orleans.Grain, IVisitTracker
    {
        private readonly ILogger<VisitTracker> _logger;
        private readonly IPersistentState<VisitTrackerState> _visitTrackerState;

        public VisitTracker(ILogger<VisitTracker> logger,
            [PersistentState(nameof(VisitTracker))]IPersistentState<VisitTrackerState> visitTrackerState)
        {
            _logger = logger;
            _visitTrackerState = visitTrackerState;
        }

        public Task<int> GetNumberOfVisits()
        {
            return Task.FromResult(_visitTrackerState.State.NumberOfVisits);
        }

        public async Task VisitAsync()
        {
            var now = DateTime.Now;
            if (!_visitTrackerState.State.FirstVisit.HasValue)
            {
                _visitTrackerState.State.FirstVisit = now;
            }

            _visitTrackerState.State.NumberOfVisits++;
            _visitTrackerState.State.LastVisit = now;
            _logger.LogInformation($"Visit at {now}");
            await _visitTrackerState.WriteStateAsync();
        }
    }
}
