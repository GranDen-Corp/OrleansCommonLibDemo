using Orleans;
using System.Threading.Tasks;

namespace HelloWorld.ShareInterface
{
    public interface IVisitTracker : IGrainWithStringKey
    {
        Task<int> GetNumberOfVisits();
        Task VisitAsync();
    }
}
