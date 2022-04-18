using Adnc.Application.Shared.Dtos;

namespace Adnc.Whse.Application.Contracts.Dtos
{
    public class ProductSearchPagedDto : SearchPagedDto
    {
        public long Id { get; set; }

        public int StatusCode { get; set; }
    }
}