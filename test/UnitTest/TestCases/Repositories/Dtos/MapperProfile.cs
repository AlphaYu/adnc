using Adnc.UnitTest.TestCases.Repositories.Entities;
using AutoMapper;

namespace Adnc.UnitTest.TestCases.Repositories.Dtos
{
    public sealed class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CustomerDto, Customer>();
        }
    }
}