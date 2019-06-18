using GranDen.Orleans.Server.SharedInterface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans.ApplicationParts;
using System;

namespace MyReminder.Grains
{
    public class MyReminderGrainServiceConfigure : IGrainServiceConfigDelegate
    {
        public Action<IApplicationPartManager> AppPartConfigurationAction =>
            part =>
            {
                part.AddDynamicPart(typeof(OutputMsg).Assembly);
            };

        public Action<HostBuilderContext, IServiceCollection> ServiceConfigurationAction =>
            (ctx, service) =>
            {
                service.AddTransient<IOutputMsg, OutputMsg>();
            };
    }
}
