using System.Threading.Tasks;
using HelloWorld.ShareInterface;
using Microsoft.Extensions.Logging;
using VisitTracker.Interface;

namespace HelloWorld.Grains
{
    public class HelloGrain : Orleans.Grain, IHello
    {
        private readonly ILogger<HelloGrain> _logger;
        private readonly IGreeter _greeter;

        public HelloGrain(ILogger<HelloGrain> logger, IGreeter greeter)
        {
            _logger = logger;
            _greeter = greeter;
        }

        public async Task<string> SayHello(string greeting)
        {
            _logger.LogInformation("HelloGrain receive RPC method invocation request");
            var ret = _greeter.DoGreeting(greeting);

            _logger.LogInformation("Call VisitTracker to record calling counts");
            var visitTracker = GrainFactory.GetGrain<IVisitTracker>(ret);
            await visitTracker.VisitAsync();

            var visitTimes = await visitTracker.GetNumberOfVisits();

            if (visitTimes > 1)
            {
                ret += $" Visit {visitTimes} times!";
            }
            
            return ret;
        }
    }
}