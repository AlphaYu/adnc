using System.Threading.Channels;

namespace Adnc.Shared.Application.Channels
{
    public sealed class Accessor<TModel>
    {
        private static readonly Lazy<Accessor<TModel>> lazy = new(() => new Accessor<TModel>());

        private readonly ChannelWriter<TModel> _writer;
        private readonly ChannelReader<TModel> _reader;

        static Accessor()
        {
        }

        private Accessor()
        {
            var channelOptions = new BoundedChannelOptions(1000)
            {
                FullMode = BoundedChannelFullMode.DropOldest
            };
            var channel = Channel.CreateBounded<TModel>(channelOptions);
            _writer = channel.Writer;
            _reader = channel.Reader;
        }

        public static Accessor<TModel> Instance => lazy.Value;

        public ChannelWriter<TModel> Writer => _writer;

        public ChannelReader<TModel> Reader => _reader;
    }
}