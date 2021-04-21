using Adnc.Application.Shared.Dtos;

namespace Adnc.Cus.Application.Contracts.Dtos
{
    public class CustomerSearchPagedDto: SearchPagedDto
    {
        public string Id { get; set; }

        public string Account { get; set; }
    }
}
