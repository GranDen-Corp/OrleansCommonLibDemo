using System;

namespace HelloWorld.Grain.Tracker
{
    public class VisitTrackerState
    {
        public DateTime? FirstVisit { get; set; }
        public DateTime? LastVisit { get; set; }
        public int NumberOfVisits { get; set; }
    }
}