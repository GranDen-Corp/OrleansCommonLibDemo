using System;
using GranDen.Orleans.Server.SharedInterface;
using HelloWorld.ShareInterface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.ApplicationParts;

namespace HelloWorld.Grain.Hello
{
    // ReSharper disable once UnusedMember.Global
    public class HelloGrainServiceConfigure : AbstractServiceConfigDelegate<HelloGrain>
    {
        public override Action<IApplicationPartManager> AppPartConfigurationAction =>
            part =>
            {
                part
                    .AddDynamicPart(typeof(VisitTracker.ShareInterface.IVisitTracker).Assembly)
                    .AddDynamicPart(typeof(IHello).Assembly)
                    .AddDynamicPart(typeof(HelloGrain).Assembly);
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
