using System.Threading.Channels;

namespace Adnc.Shared.Application.Channels
{
    public sealed class LogAccessor<TModel>
    {
        private static readonly Lazy<LogAccessor<TModel>> lazy = new(() => new LogAccessor<TModel>());

        private readonly ChannelWriter<TModel> _writer;
        private readonly ChannelReader<TModel> _reader;

        static LogAccessor()
        {
        }

        private LogAccessor()
        {
            var channelOptions = new BoundedChannelOptions(1000)
            {
                FullMode = BoundedChannelFullMode.DropOldest
            };
            var channel = Channel.CreateBounded<TModel>(channelOptions);
            _writer = channel.Writer;
            _reader = channel.Reader;
        }

        public static LogAccessor<TModel> Instance => lazy.Value;

        public ChannelWriter<TModel> Writer => _writer;

        public ChannelReader<TModel> Reader => _reader;
    }
}