using System;
using System.Threading.Tasks;
using GranDen.Orleans.Server.SharedInterface;
using HelloWorld.ShareInterface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: Orleans.CodeGeneration.KnownAssembly(typeof(IHello))]
namespace HelloWorld.Grains
{
    public class HelloGrain : Orleans.Grain, IHello
    {
        private readonly ILogger<HelloGrain> _logger;
        private readonly Greeter _greeter;

        public HelloGrain(ILogger<HelloGrain> logger, Greeter greeter)
        {
            _logger = logger;
            _greeter = greeter;
        }

        public Task<string> SayHello(string greeting)
        {
            _logger.LogInformation("HelloGrain receive RPC method invocation request");
            var ret = _greeter.DoGreeting(greeting);
            return Task.FromResult(ret);
        }
    }
}