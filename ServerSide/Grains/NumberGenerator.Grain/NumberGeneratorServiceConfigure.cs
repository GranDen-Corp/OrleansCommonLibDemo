using System;
using GranDen.Orleans.Server.SharedInterface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans.ApplicationParts;

namespace NumberGenerator.Grain
{
    // ReSharper disable once UnusedMember.Global
    public class NumberGeneratorServiceConfigure : AbstractServiceConfigDelegate<NumberGeneratorGrain>
    {
        public override Action<IApplicationPartManager> AppPartConfigurationAction =>
            part =>
            {
                part
                    .AddDynamicPart(typeof(ShareInterface.INumberGenerator).Assembly)
                    .AddDynamicPart(typeof(NumberGeneratorGrain).Assembly);
            };

        public override Action<HostBuilderContext, IServiceCollection> ServiceConfigurationAction =>
            (ctx, service) => { };
    }
}
