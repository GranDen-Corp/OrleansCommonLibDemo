using Orleans.Runtime;

namespace MyReminder.Grains
{
    public class ReminderInfo
    {
        public int CalledCount { get; set; } = 0;
        public IGrainReminder Reminder { get; set; }
    }
}
