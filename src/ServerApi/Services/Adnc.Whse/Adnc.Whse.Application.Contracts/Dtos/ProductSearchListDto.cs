using Adnc.Application.Shared.Dtos;

namespace Adnc.Whse.Application.Contracts.Dtos
{
    public class ProductSearchListDto : SearchDto
    {
        public string[] Ids { get; set; }

        public int StatusCode { get; set; }
    }
}