﻿using GranDen.Orleans.Server.SharedInterface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans.ApplicationParts;
using System;
using System.Linq;
using System.Reflection;
using Orleans;

namespace HelloWorld.Grains
{
    public class HelloGrainServiceConfigure : IGrainServiceConfigDelegate
    {
        public Action<IApplicationPartManager> AppPartConfigurationAction =>
            part =>
            {
                part.AddDynamicPart(typeof(IGreeter).Assembly);
            };

        public Action<HostBuilderContext, IServiceCollection> ServiceConfigurationAction =>
            (ctx, service) =>
            {
                //var apm = ctx.GetApplicationPartManager();
                //apm.AddApplicationPart(typeof(IGreeter).Assembly);

                service.AddTransient<IGreeter, Greeter>();
            };
    }
}
