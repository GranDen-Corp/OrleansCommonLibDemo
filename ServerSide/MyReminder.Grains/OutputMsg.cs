using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyReminder.Grains
{
    public class OutputMsg : IOutputMsg
    {
        private readonly ILogger<MyReminderGrain> _logger;

        public OutputMsg(ILogger<MyReminderGrain> logger)
        {
            _logger = logger;
        }

        public void Output(string msg)
        {
            _logger.LogInformation(msg);
        }
    }

    public interface IOutputMsg
    {
        void Output(string msg);
    }
}
