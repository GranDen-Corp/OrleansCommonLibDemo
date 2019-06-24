using GranDen.Orleans.Server.SharedInterface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans.ApplicationParts;
using System;
using Microsoft.Extensions.Logging;

namespace HelloWorld.Grains
{
    public class HelloGrainServiceConfigure : AbstractServiceConfigDelegate
    {
        public override Action<IApplicationPartManager> AppPartConfigurationAction =>
            part =>
            {
                part.AddDynamicPart(typeof(Greeter).Assembly);
            };

        public override Action<HostBuilderContext, IServiceCollection> ServiceConfigurationAction =>
        (ctx, services) =>
        {
            services.AddTransient<IGreeter, Greeter>(provider =>
            {
                var logger = provider.GetService<ILogger<Greeter>>();
                return new Greeter(logger);
            });
        };
    }
}
