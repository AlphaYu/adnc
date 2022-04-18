using Adnc.Application.Shared.Dtos;

namespace Adnc.Whse.Application.Contracts.Dtos
{
    public class ProductSearchListDto : SearchDto
    {
        public long[] Ids { get; set; }

        public int StatusCode { get; set; }
    }
}