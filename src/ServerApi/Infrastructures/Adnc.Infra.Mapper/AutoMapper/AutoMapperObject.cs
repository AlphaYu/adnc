using Adnc.Infra.Core.Guard;
using AutoMapper;

namespace Adnc.Infra.Mapper.AutoMapper
{
    public sealed class AutoMapperObject : IObjectMapper
    {
        private readonly IMapper _mapper;

        public AutoMapperObject(IMapper mapper) => _mapper = mapper;

        public TDestination Map<TDestination>(object source)
        {
            Checker.Argument.IsNotNull(source, nameof(source));
            return _mapper.Map<TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source)
            where TSource : class
            where TDestination : class
        {
            Checker.Argument.IsNotNull(source, nameof(source));
            return _mapper.Map<TSource, TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
          where TSource : class
          where TDestination : class
        {
            Checker.Argument.IsNotNull(source, nameof(source));
            Checker.Argument.IsNotNull(destination, nameof(destination));
            return _mapper.Map(source, destination);
        }
    }
}