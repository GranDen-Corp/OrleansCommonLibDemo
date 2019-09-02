using System;
using GranDen.Orleans.Server.SharedInterface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans.CodeGeneration;

[assembly: KnownAssembly(typeof(MyReminder.ShareInterface.IMyReminder))]

namespace MyReminder.Grain
{
    // ReSharper disable once UnusedMember.Global
    public class MyReminderGrainServiceConfigure : AbstractServiceConfigDelegate<MyReminderGrain>
    {
        public override Action<HostBuilderContext, IServiceCollection> ServiceConfigurationAction =>
            (ctx, service) =>
            {
                service.AddTransient<IOutputMsg, OutputMsg>();
            };
    }
}
