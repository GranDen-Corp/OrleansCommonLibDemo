using GranDen.Orleans.Server.SharedInterface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans.ApplicationParts;
using System;

namespace HelloWorld.Grains
{
    public class HelloGrainServiceConfigure : IGrainServiceConfigDelegate
    {
        public Action<IApplicationPartManager> AppPartConfigurationAction =>
            part => part.AddDynamicPart(typeof(Greeter).Assembly);

        Action<HostBuilderContext, IServiceCollection> IServiceConfigDelegate.ServiceConfigurationAction =>
            (ctx, service) =>
            {
                service.AddTransient<Greeter>();
            };
    }
}
