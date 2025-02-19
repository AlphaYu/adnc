using Adnc.Infra.Unittest.Reposity.Fixtures.Entities;

namespace Adnc.Infra.Unittest.Reposity.Fixtures.Dtos;
public sealed class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<CustomerDto, Customer>();
    }
}