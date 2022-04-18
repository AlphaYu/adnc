using Adnc.Infra.Mapper;

namespace Adnc.Application.Shared.Services
{
    public interface IAppService
    {
        IObjectMapper Mapper { get; set; }
    }
}