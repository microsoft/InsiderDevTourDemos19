using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Builder;

namespace StreamR
{
    public class StreamManager
    {
			private readonly ConcurrentDictionary<string, StreamHolder> _streams = new ConcurrentDictionary<string, StreamHolder>();
        private long _globalClientId;
       
        public List<string> ListStreams()
        {
            var streamList = new List<string>();
			return streamList;
        }

        public async Task RunStreamAsync(string streamName, ChannelReader<string> stream) {
            var streamHolder = new StreamHolder() { Source = stream };
			
			// Add before yielding
            // This fixes a race where we tell clients a new stream arrives before adding the stream
            this._streams.TryAdd(streamName, streamHolder);

            await Task.Yield();

	    try
            {
                await foreach (var item in stream.ReadAllAsync()) {
		        foreach (var viewer in streamHolder.Viewers)
                    {
                        try
                        {
                            await viewer.Value.Writer.WriteAsync(item);
                        }
                        catch { }
                    }
                }
            }
            finally
            {
                RemoveStream(streamName);
            }
        }

        public void RemoveStream(string streamName)
        {
            this._streams.TryRemove(streamName, out var streamHolder);
            foreach (var viewer in streamHolder.Viewers)
            {
				viewer.Value.Writer.TryComplete();
            }
        }

        public IAsyncEnumerable<string> Subscribe(string streamName, CancellationToken cancellationToken)
        {
            if (!this._streams.TryGetValue(streamName, out var source))
            {
                    throw new HubException("stream doesn't exist");
            }

            var     id = Interlocked.Increment(ref this._globalClientId);

            var channel = Channel.CreateBounded<string>(options: new BoundedChannelOptions(2)
            {
                FullMode = BoundedChannelFullMode.DropOldest
            });

            source.Viewers.TryAdd(id, channel);

            // Register for client closing stream, this token will always fire (handled by SignalR)
            cancellationToken.Register(() =>
            {
                source.Viewers.TryRemove(id, out _);
            });

            return  channel.Reader.ReadAllAsync();
        }

        private class StreamHolder
        {
            public  ChannelReader<string> Source;
            public ConcurrentDictionary<long, Channel<string>> Viewers = new ConcurrentDictionary<long, Channel<string>>();
        }
    }
}