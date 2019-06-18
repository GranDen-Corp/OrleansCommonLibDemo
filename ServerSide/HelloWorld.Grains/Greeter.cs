using Microsoft.Extensions.Logging;

namespace HelloWorld.Grains
{
    public class Greeter : IGreeter
    {
        private readonly ILogger<Greeter> _logger;

        public Greeter(ILogger<Greeter> logger)
        {
            _logger = logger;
        }

        public string DoGreeting(string greeting)
        {
            _logger.LogInformation($"SayHello message received: greeting = '{greeting}'");
            return $"You said: '{greeting}', I say: Hello!";
        }
    }

    public interface IGreeter
    {
        string DoGreeting(string greeting);
    }
}