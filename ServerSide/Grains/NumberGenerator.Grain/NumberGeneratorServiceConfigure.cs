using System;
using System.Collections.Generic;
using System.Text;
using GranDen.Orleans.Server.SharedInterface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NumberGenerator.ShareInterface;
using Orleans.CodeGeneration;

[assembly: KnownAssembly(typeof(INumberGenerator))]

namespace NumberGenerator.Grain
{
    // ReSharper disable once UnusedMember.Global
    public class NumberGeneratorServiceConfigure : AbstractServiceConfigDelegate<NumberGeneratorGrain>
    {
        public override Action<HostBuilderContext, IServiceCollection> ServiceConfigurationAction { get; }
    }
}
