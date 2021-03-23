using System.Threading.Channels;
using System.Threading.Tasks;

namespace Microservices.Common.EventChannel
{
    public static class EventDispatchChannel<T> where T : class
    {
        static Channel<T> _channel;
        static EventDispatchChannel()
        {
            var options = new BoundedChannelOptions(int.MaxValue)
            {
                SingleWriter = false,
                SingleReader = true
            };

            _channel = Channel.CreateBounded<T>(options);
        }

        public static async Task<bool> PushAsync(T dto)
        {
            while (await _channel.Writer.WaitToWriteAsync())
                if (_channel.Writer.TryWrite(dto))
                    return true;
            return false;
        }

        public static async Task<T> PullAsync() => await _channel.Reader.ReadAsync();
    }
}