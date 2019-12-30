using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using GranDen.Orleans.NetCoreGenericHost.CommonLib;
using McMaster.NETCore.Plugins;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace LocalConsoleSiloHost
{
    class Program
    {
        static void Main(string[] args)
        {
            //SetupTopLogger();

            var genericHostBuilder =
                OrleansSiloBuilderExtension.CreateHostBuilder(args, configFilePrefix: "appsettings")
                                           .ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) => 
                                           {
                                               if(hostBuilderContext.HostingEnvironment.IsDevelopment())
                                               {
                                                   configurationBuilder.AddUserSecrets<Program>();
                                               }
                                           })
                                           .ConfigureServices((hostContext, services) =>
                                           {
                                               services.AddApplicationInsightsTelemetryWorkerService();
                                           })
                                           .UseSerilog((hostBuilderContext, loggerConfiguration) =>
                                           {
                                               loggerConfiguration.ReadFrom.Configuration(hostBuilderContext.Configuration);

                                               var telemetryClient = CreateTelemetryClient(hostBuilderContext);
                                               if (telemetryClient != null)
                                               {
                                                   loggerConfiguration.WriteTo.ApplicationInsights(telemetryClient, TelemetryConverter.Traces);
                                               }
                                           });

#if DEBUG
            genericHostBuilder.UseEnvironment(Environments.Development);
#endif

            try
            {
                var genericHost = genericHostBuilder.Build();
                PluginCache = OrleansSiloBuilderExtension.PlugInLoaderCache;
                genericHost.Run();
            }
            catch (OperationCanceledException exception)
            {
                //do nothing
                Log.Information(exception, "Orleans operation cancelled");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Orleans Silo Host error");
                throw;
            }
            finally
            {
                foreach (var kv in PluginCache)
                {
                    kv.Value.Dispose();
                }
            }
        }

        public static Dictionary<string, PluginLoader> PluginCache { get; set; }

        private static TelemetryClient CreateTelemetryClient(HostBuilderContext hostBuilderContext)
        {
            var key = hostBuilderContext.Configuration.GetValue<string>("ApplicationInsights:InstrumentationKey");
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            var provider = new ServiceCollection()
                .AddApplicationInsightsTelemetryWorkerService(key).BuildServiceProvider();
            var telemetryClient = provider.GetRequiredService<TelemetryClient>();
            telemetryClient.InstrumentationKey = key;
            return telemetryClient;
        }
    }
}
