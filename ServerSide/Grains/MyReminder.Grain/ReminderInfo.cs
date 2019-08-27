using Orleans.Runtime;

namespace MyReminder.Grain
{
    public class ReminderInfo
    {
        public int CalledCount { get; set; } = 0;
        public IGrainReminder Reminder { get; set; }
    }
}
