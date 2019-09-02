using System;
using GranDen.Orleans.Server.SharedInterface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.CodeGeneration;

[assembly: KnownAssembly(typeof(HelloWorld.ShareInterface.IHello))]
[assembly: KnownAssembly(typeof(VisitTracker.Interface.IVisitTracker))]

namespace HelloWorld.Grain.Hello
{
    // ReSharper disable once UnusedMember.Global
    public class HelloGrainServiceConfigure : AbstractServiceConfigDelegate<HelloGrain>
    {
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
