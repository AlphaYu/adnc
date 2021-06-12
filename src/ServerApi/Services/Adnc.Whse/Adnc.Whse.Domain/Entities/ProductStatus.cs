using Adnc.Domain.Shared.Entities;

namespace Adnc.Whse.Domain.Entities
{
    public class ProductStatus : ValueObject
    {
        public ProductStatusEnum Code { get; }

        public string ChangesReason { get; }

        private ProductStatus()
        {
        }

        internal ProductStatus(ProductStatusEnum statusCode, string reason)
        {
            Code = statusCode;
            ChangesReason = reason != null ? reason.Trim() : string.Empty;
        }
    }

    public enum ProductStatusEnum
    {
        UnKnow = 1000
        ,
        SaleOff = 1008
        ,
        SaleOn = 1016
    }
}