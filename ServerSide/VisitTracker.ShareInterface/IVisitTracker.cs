using Orleans;
using System.Threading.Tasks;

namespace VisitTracker.ShareInterface
{
    public interface IVisitTracker : IGrainWithStringKey
    {
        Task<int> GetNumberOfVisits();
        Task VisitAsync();
    }
}
