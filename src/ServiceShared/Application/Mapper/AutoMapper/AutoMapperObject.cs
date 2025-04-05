using Adnc.Infra.Core.Guard;
using AutoMapper;

namespace Adnc.Shared.Application.Mapper.AutoMapper;

public sealed class AutoMapperObject(IMapper mapper) : IObjectMapper
{
    public TDestination Map<TDestination>(object source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        return mapper.Map<TDestination>(source);
    }

    public TDestination Map<TSource, TDestination>(TSource source)
        where TSource : class
        where TDestination : class
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        return mapper.Map<TSource, TDestination>(source);
    }

    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
      where TSource : class
      where TDestination : class
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(destination, nameof(destination));
        return mapper.Map(source, destination);
    }

    public TDestination Map<TDestination>(object source, long id)
        where TDestination : Entity
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        Checker.Argument.ThrowIfLEZero(id, nameof(id));
        var destination = mapper.Map<TDestination>(source);
        destination.Id = id;
        return destination;
    }
}
