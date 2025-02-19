using AutoMapper;
using Adnc.Infra.Core.Guard;

namespace Adnc.Infra.Mapper.AutoMapper
{
    public sealed class AutoMapperObject(IMapper mapper) : IObjectMapper
    {
        public TDestination Map<TDestination>(object source)
        {
            Checker.Argument.NotNull(source, nameof(source));
            return mapper.Map<TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source)
            where TSource : class
            where TDestination : class
        {
            Checker.Argument.NotNull(source, nameof(source));
            return mapper.Map<TSource, TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
          where TSource : class
          where TDestination : class
        {
            Checker.Argument.NotNull(source, nameof(source));
            Checker.Argument.NotNull(destination, nameof(destination));
            return mapper.Map(source, destination);
        }
    }
}