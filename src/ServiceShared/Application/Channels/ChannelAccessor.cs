using System.Threading.Channels;

namespace Adnc.Shared.Application.Channels;

public sealed class ChannelAccessor<TModel>
{
    private static readonly Lazy<ChannelAccessor<TModel>> _lazy = new(() => new ChannelAccessor<TModel>());
    private readonly ChannelReader<TModel> _reader;

    private readonly ChannelWriter<TModel> _writer;

    static ChannelAccessor()
    {
    }

    private ChannelAccessor()
    {
        var channelOptions = new BoundedChannelOptions(1000)
        {
            FullMode = BoundedChannelFullMode.DropOldest
        };
        var channel = Channel.CreateBounded<TModel>(channelOptions);
        _writer = channel.Writer;
        _reader = channel.Reader;
    }

    public static ChannelAccessor<TModel> Instance => _lazy.Value;

    public ChannelWriter<TModel> Writer => _writer;

    public ChannelReader<TModel> Reader => _reader;
}
