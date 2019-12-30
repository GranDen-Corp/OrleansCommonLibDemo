using System.Threading.Tasks;
using Orleans;

namespace NumberGenerator.ShareInterface
{
    public interface INumberGenerator : IGrainWithGuidKey
    {
        Task<int> NextInt();

        Task<double> NextDouble();

        Task<decimal> NextDecimal();
    }
}