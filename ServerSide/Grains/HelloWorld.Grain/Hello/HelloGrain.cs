using System.Threading.Tasks;
using HelloWorld.ShareInterface;
using Microsoft.Extensions.Logging;
using VisitTracker.Interface;

namespace HelloWorld.Grain.Hello
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
            var greetingMsg = _greeter.DoGreeting(greeting);

            var visitTimes = await CallVisitTracker(greetingMsg);

            if (visitTimes <= 1) { return greetingMsg; }
            greetingMsg += $" Visit {visitTimes} times!";
            _logger.LogInformation($"Say Hello {visitTimes} times!");

            return greetingMsg;
        }

        private async Task<int> CallVisitTracker(string ret)
        {
            _logger.LogInformation("Call VisitTracker to record This invocation counts");
            var visitTracker = GrainFactory.GetGrain<IVisitTracker>(ret);
            await visitTracker.VisitAsync();

            var visitTimes = await visitTracker.GetNumberOfVisits();
            return visitTimes;
        }
    }
}