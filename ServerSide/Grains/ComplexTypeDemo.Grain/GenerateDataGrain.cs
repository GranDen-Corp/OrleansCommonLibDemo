using System;
using System.Threading.Tasks;
using ComplexTypeDemo.ShareInterface;
using Microsoft.Extensions.Logging;
using NumberGenerator.ShareInterface;
using Orleans.CodeGeneration;

namespace ComplexTypeDemo.Grain
{
    [GrainReference(typeof(ICreateRecord))]
    public class GenerateDataGrain : Orleans.Grain, ICreateRecord
    {
        private readonly ILogger<GenerateDataGrain> _logger;

        public GenerateDataGrain(ILogger<GenerateDataGrain> logger)
        {
            _logger = logger;
        }

        public async Task<ComplexCollection> CreateRandomData()
        {
            _logger.LogInformation($"Invoke {nameof(CreateRandomData)}() RPC method...");

            try
            {
                var randomGrain = GrainFactory.GetGrain<INumberGenerator>(typeof(INumberGenerator),Guid.Empty);

                var ret = new ComplexCollection();

                for (var i = 0; i < 5; i++)
                {
                    ret.Add(await CreateMyTypeObj(randomGrain));
                }

                return ret;
            }
            catch (Exception e)
            {
                _logger.LogError(e,"run error");
                throw;
            }
        }

        private async Task<MyType> CreateMyTypeObj(INumberGenerator randomGrain)
        {
            var randomInt = await randomGrain.NextInt();
            _logger.LogInformation($"random = {randomInt}");
            return new MyType
            {
                ID = Guid.NewGuid(),
                Message = $"dummy data-{randomInt}",
                StatusCode = randomInt,
            };
        }
    }
}
