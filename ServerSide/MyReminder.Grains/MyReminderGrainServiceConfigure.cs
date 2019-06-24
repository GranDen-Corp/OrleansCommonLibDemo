using GranDen.Orleans.Server.SharedInterface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans.ApplicationParts;
using System;

namespace MyReminder.Grains
{
    public class MyReminderGrainServiceConfigure : AbstractServiceConfigDelegate
    {
        public override Action<IApplicationPartManager> AppPartConfigurationAction =>
            part =>
            {
                part.AddDynamicPart(typeof(OutputMsg).Assembly);
            };

        public override Action<HostBuilderContext, IServiceCollection> ServiceConfigurationAction =>
            (ctx, service) =>
            {
                service.AddTransient<IOutputMsg, OutputMsg>();
            };
    }
}
