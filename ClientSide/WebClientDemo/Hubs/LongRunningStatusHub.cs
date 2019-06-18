using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using GranDen.Orleans.Client.CommonLib;
using GranDen.Orleans.Client.CommonLib.TypedOptions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyReminder.ShareInterface;
using NUlid;
using Orleans.Runtime;

namespace WebClientDemo.Hubs
{
    public class LongRunningStatusHub : Hub
    {
        private readonly ILogger<LongRunningStatusHub> _logger;
        private readonly ClusterInfoOption _clusterInfo;
        private readonly OrleansProviderOption _providerOption;

        public LongRunningStatusHub(ILogger<LongRunningStatusHub> logger, 
            IOptionsMonitor<ClusterInfoOption> clusterInfoOptionsMonitor, IOptionsMonitor<OrleansProviderOption> providerOptionsMonitor)
        {
            _logger = logger;
            _clusterInfo = clusterInfoOptionsMonitor.CurrentValue;
            _providerOption = providerOptionsMonitor.CurrentValue;
        }

        // ReSharper disable once UnusedMember.Global
        public ChannelReader<string> CheckJobStatus(Ulid grainId)
        {
            var channel = Channel.CreateUnbounded<string>();
            _ = DetectLongRunningTaskStatus(channel.Writer, grainId, 3, new CancellationToken());

            return channel.Reader;
        }

        #region Private Methods

        private async Task DetectLongRunningTaskStatus(ChannelWriter<string> writer, Ulid grainId, int delay,CancellationToken cancellationToken)
        {
            try
            {
                using (var client = OrleansClientBuilder.CreateClient(_logger, _clusterInfo, _providerOption,
                    new[] {typeof(IMyReminder)}))
                {
                    await client.ConnectWithRetryAsync();
                    var grain = client.GetGrain<IMyReminder>(grainId.ToGuid());

                    string status;
                    do
                    {
                        // Check the cancellation token regularly so that the server will stop
                        // producing items if the client disconnects.
                        cancellationToken.ThrowIfCancellationRequested();

                        status = await grain.GetCurrentStatus();

                        await writer.WriteAsync(status, cancellationToken);
                        if (status == "stopped")
                        {
                            break;
                        }

                        await Task.Delay(TimeSpan.FromSeconds(delay), cancellationToken);

                    } while (status == "running");

                    await client.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(400, "Runtime error", ex);
                writer.TryComplete(ex);
                return;
            }

            writer.TryComplete();
        }

        #endregion
    }
}
