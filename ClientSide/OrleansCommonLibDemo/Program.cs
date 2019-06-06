using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace OrleansCommonLibDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = SetupDI();

            var logger = GetLogger<Program>(serviceProvider);

            logger.LogInformation("Press space key to run demo");
            do
            {
                while (!Console.KeyAvailable)
                {
                    //wait
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Spacebar);

            try
            {
                var demo = serviceProvider.GetService<CallGrainDemo>();
                await demo.CallHello();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "error occured!");
                throw;
            }

            logger.LogInformation("Press enter to exit");
            Console.ReadLine();
        }

        private static ServiceProvider SetupDI()
        {
            var services = new ServiceCollection();
            services.AddLogging(config => config.AddConsole());
            services.AddTransient<CallGrainDemo>();

            return services.BuildServiceProvider();
        }

        private static ILogger<T> GetLogger<T>(ServiceProvider serviceProvider)
        {
            return serviceProvider.GetService<ILogger<T>>();
        }
    }
}
