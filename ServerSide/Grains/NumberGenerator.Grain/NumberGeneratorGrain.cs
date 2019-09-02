using System.Threading.Tasks;
using NumberGenerator.ShareInterface;

namespace NumberGenerator.Grain
{
    public class NumberGeneratorGrain : Orleans.Grain, INumberGenerator
    {
        public Task<int> NextInt()
        {
            return Task.FromResult(RandomLib.RandomNumberGenerator.CreateRandomInt(0, 100));
        }

        public Task<double> NextDouble()
        {
            return Task.FromResult(RandomLib.RandomNumberGenerator.CreateRandomDouble(0.0, 100.0));
        }

        public async Task<decimal> NextDecimal()
        {
            return new decimal(await NextDouble());
        }
    }
}
