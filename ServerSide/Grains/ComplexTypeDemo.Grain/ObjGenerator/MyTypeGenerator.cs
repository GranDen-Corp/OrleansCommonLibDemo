using System;
using System.Threading.Tasks;
using ComplexTypeDemo.ShareInterface;
using Microsoft.Extensions.Logging;
using NumberGenerator.ShareInterface;
using Orleans;

namespace ComplexTypeDemo.Grain.ObjGenerator
{
    //public class MyTypeGenerator : ICreateMyType
    //{
    //    private readonly ILogger<MyTypeGenerator> _logger;
            
    //    private readonly INumberGenerator _randomGrain;

    //    public MyTypeGenerator(ILogger<MyTypeGenerator> logger, IGrainFactory grainFactory)
    //    {
    //        _logger = logger;
    //        _randomGrain = grainFactory.GetGrain<INumberGenerator>(Guid.Empty);
    //    }

    //    public async Task<MyType> CreateMyTypeObj()
    //    {
    //        var randomInt = await _randomGrain.NextInt();
    //        _logger.LogInformation($"random = {randomInt}");
    //        return new MyType
    //        {
    //            ID = Guid.NewGuid(),
    //            Message = $"dummy data-{randomInt}",
    //            StatusCode = randomInt,
    //        };
    //    }
    //}
}
