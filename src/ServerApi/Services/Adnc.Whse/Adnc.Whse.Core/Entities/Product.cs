using Adnc.Infr.Common.Exceptions;
using Adnc.Core.Shared.Entities;

namespace Adnc.Whse.Core.Entities
{
    public class Product : AggregateRootWithBasicAuditInfo
    {
        public string Sku { get; private set; }

        public string Name { get; private set; }

        public string Describe { set; get; }

        public decimal Price { get; private set; }

        public ProductStatus Status { get; private set; }

        public string Unit { get; private set; }

        private Product() { }

        /// <summary>
        /// 创建商品需要依赖仓储判断是否存在同名，所以必须要在领域服务类处理部分业务逻辑
        /// internal可以防止应用服务直接使用Product的构造函数去创建实例,限制必须使用ProductManager来创建.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sku"></param>
        /// <param name="name"></param>
        /// <param name="unit"></param>
        /// <param name="describe"></param>
        internal Product(long id, string sku, decimal price, string name, string unit, string describe = null)
        {
            this.Id = id;
            SetSku(sku);
            SetName(name);
            SetPrice(price);
            SetUnit(unit);
            this.Describe = describe != null ? describe.Trim() : string.Empty;
            this.Status = new ProductStatus(ProductStatusEnum.UnKnow, string.Empty);
        }

        /// <summary>
        /// 设置sku
        /// </summary>
        /// <param name="sku"></param>
        internal void SetSku(string sku)
        {
            this.Sku = Checker.NotNullOrEmpty(sku.Trim(), nameof(sku));
        }

        /// <summary>
        /// 设置Name
        /// </summary>
        /// <param name="name"></param>
        internal void SetName(string name)
        {
            this.Name = Checker.NotNullOrEmpty(name.Trim(), nameof(name));
        }


        /// <summary>
        /// 设置商品状态
        /// </summary>
        /// <param name="status"></param>
        internal void SetStatus(ProductStatus status)
        {
            this.Status = Checker.NotNull(status, nameof(status));
        }

        /// <summary>
        /// 修改unit
        /// </summary>
        /// <param name="newSku"></param>
        public void SetUnit(string unit)
        {
            this.Unit = Checker.NotNullOrEmpty(unit.Trim(), nameof(unit));
        }

        /// <summary>
        /// 修改Price
        /// </summary>
        /// <param name="price"></param>
        public void SetPrice(decimal price)
        {
            if (price <= 0)
                throw new AdncArgumentException("不能小于等于0", nameof(price));

            this.Price = price;
        }

        /// <summary>
        /// 下架商品，不允许销售
        /// </summary>
        public void PutOffSale(string reason)
        {
            this.Status = new ProductStatus(ProductStatusEnum.SaleOff, reason);
        }
    }
}
