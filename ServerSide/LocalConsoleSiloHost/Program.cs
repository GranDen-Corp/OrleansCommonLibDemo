﻿using Microsoft.Extensions.Hosting;
using System;
using GranDen.Orleans.NetCoreGenericHost.CommonLib;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.SystemConsole.Themes;

namespace LocalConsoleSiloHost
{
    class Program
    {
        static void Main(string[] args)
        {
            SetupLogger();

            var genericHostBuilder = OrleansSiloBuilderExtension.CreateHostBuilder(args).ApplySerilog();

            try
            {
                var genericHost = genericHostBuilder.Build();
                genericHost.Run();
            }
            catch (OperationCanceledException exception)
            {
                //do nothing
                Log.Information(exception,"Orleans operation cancelled");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Orleans Silo Host error");
                throw;
            }
        }

        private static void SetupLogger()
        {
            var logConfig = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Orleans.Runtime.Management.ManagementGrain", LogEventLevel.Warning)
                .MinimumLevel.Override("Orleans.Runtime.SiloControl", LogEventLevel.Warning)
                .MinimumLevel.Override("Orleans.Runtime.MembershipService.MembershipTableManager", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                .Enrich.WithThreadId()
                .Enrich.WithExceptionDetails()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .WriteTo.Trace()
                .WriteTo.Debug();

            Log.Logger = logConfig.CreateLogger();
        }
    }
}
