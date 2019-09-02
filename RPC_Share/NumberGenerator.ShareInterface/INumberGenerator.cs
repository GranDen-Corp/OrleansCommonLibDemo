using System.Threading.Tasks;
using Orleans;

namespace NumberGenerator.ShareInterface
{
    public interface INumberGenerator : IGrainWithStringKey
    {
        Task<int> NextInt();

        Task<double> NextDouble();

        Task<decimal> NextDecimal();
    }
}