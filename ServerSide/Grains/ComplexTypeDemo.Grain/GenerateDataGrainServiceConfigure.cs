using System;
using ComplexTypeDemo.ShareInterface;
using GranDen.Orleans.Server.SharedInterface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans.ApplicationParts;

namespace ComplexTypeDemo.Grain
{
    // ReSharper disable once UnusedMember.Global
    public class GenerateDataGrainServiceConfigure : AbstractServiceConfigDelegate<GenerateDataGrain>
    {
        public override Action<IApplicationPartManager> AppPartConfigurationAction =>
            part => 
            {
                part
                .AddDynamicPart(typeof(ICreateRecord).Assembly)
                .AddDynamicPart(typeof(GenerateDataGrain).Assembly);
            };

        public override Action<HostBuilderContext, IServiceCollection> ServiceConfigurationAction =>
            (ctx, services) =>
            {
                // services.AddTransient<ICreateMyType, MyTypeGenerator>();
            };
    }
}
