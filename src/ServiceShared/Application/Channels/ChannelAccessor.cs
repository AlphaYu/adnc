using System.Threading.Channels;

namespace Adnc.Shared.Application.Channels;

public sealed class ChannelAccessor<TModel>
{
    private static readonly Lazy<ChannelAccessor<TModel>> _lazy = new(() => new ChannelAccessor<TModel>());

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
        Writer = channel.Writer;
        Reader = channel.Reader;
    }

    public static ChannelAccessor<TModel> Instance => _lazy.Value;

    public ChannelWriter<TModel> Writer { get; private set; }

    public ChannelReader<TModel> Reader { get; private set; }
}
