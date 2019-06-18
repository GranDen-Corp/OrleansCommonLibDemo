using Orleans;
using System;
using System.Threading.Tasks;

namespace MyReminder.ShareInterface
{
    public struct HelloMyValue
    {
        public string Greeting;
        public DateTime YellTime;

        public override string ToString()
        {
            return $"{{Greeting={Greeting}, YellTime={YellTime:O}}}";
        }
    }

    public interface IMyReminder : IGrainWithGuidKey
    {
        ValueTask<HelloMyValue> Alarm();
        Task<string> GetCurrentStatus();
    }
}
