using System;
using System.Threading.Tasks;
using Orleans;

namespace VisitTracker.ShareInterface
{
    public interface IVisitTracker : IGrainWithStringKey
    {
        Task<int> GetNumberOfVisits();
        Task VisitAsync();
    }
}
