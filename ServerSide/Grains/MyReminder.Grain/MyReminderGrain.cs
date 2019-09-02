using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyReminder.ShareInterface;
using Orleans;
using Orleans.Runtime;

namespace MyReminder.Grain
{
    public class MyReminderGrain : Orleans.Grain, IMyReminder, IRemindable
    {
        private readonly IOutputMsg _outputMsg;

        private readonly Dictionary<string, ReminderInfo> _registeredReminders = new Dictionary<string, ReminderInfo>();

        private const int UpperLimit = 0;
        private int _calledTimes;

        public MyReminderGrain(IOutputMsg outputMsg)
        {
            _outputMsg = outputMsg;
        }

        public async ValueTask<HelloMyValue> Alarm()
        {
            _calledTimes++;
            var reminderName = $"myReminder{_calledTimes}";
            var reminder = await RegisterOrUpdateReminder(reminderName, TimeSpan.FromSeconds(30), TimeSpan.FromMinutes(1));
            _registeredReminders[reminderName] = new ReminderInfo { Reminder = reminder, CalledCount = 1 };

            //TODO: init some long-running Task job.

            var output = new HelloMyValue { Greeting = "Hello World!", YellTime = DateTime.Now };
            _outputMsg.Output(output.Greeting);

            return output;
        }

        public Task<string> GetCurrentStatus()
        {
            return _registeredReminders.Count > 0 ? Task.FromResult(@"running") : Task.FromResult("stopped");
        }

        public async Task ReceiveReminder(string reminderName, TickStatus status)
        {
            var output = new HelloMyValue { Greeting = $"get Reminder: {reminderName}", YellTime = DateTime.Now };
            _outputMsg.Output(output.ToString());

            var reminderInfo = _registeredReminders[reminderName];

            reminderInfo.CalledCount++;

            if (reminderInfo.CalledCount >= UpperLimit)
            {
                await UnregisterReminder(reminderInfo.Reminder);
                _outputMsg.Output($"Reminder: {reminderName} unregistered.");
                _registeredReminders.Remove(reminderName);
            }
        }
    }
}
