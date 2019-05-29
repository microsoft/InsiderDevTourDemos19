using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace StreamR
{
    public class StreamHub : Hub
    {
        private readonly StreamManager _streamManager;

        public StreamHub(StreamManager streamManager)
        {
            _streamManager = streamManager;
        }

        public List<string> ListStreams()
        {
            return _streamManager.ListStreams();
        }

        public IAsyncEnumerable<string> WatchStream(string streamName, CancellationToken cancellationToken)
        {
            return _streamManager.Subscribe(streamName, cancellationToken);
        }

        public async Task StartStream(string streamName, ChannelReader<string> streamContent)
        {
            try
            {
                var streamTask = _streamManager.RunStreamAsync(streamName, streamContent);

                // Tell everyone about your stream!
                await Clients.Others.SendAsync("NewStream", streamName);

                await streamTask;
            }
            finally
            {
                await Clients.Others.SendAsync("RemoveStream", streamName);
            }
        }
    }
}