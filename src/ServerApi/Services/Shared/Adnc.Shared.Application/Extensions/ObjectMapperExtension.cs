namespace Adnc.Infra.Mapper;

public static class ObjectMapperExtension
{
    public static TDestination Map<TDestination>(this IObjectMapper mapper, object source, long id)
        where TDestination : Entity
    {
        var destination = mapper.Map<TDestination>(source);
        if (destination is null)
            return destination;

        destination.Id = id;
        return destination;
    }
}
