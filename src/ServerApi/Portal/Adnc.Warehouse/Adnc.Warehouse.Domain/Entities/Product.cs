using System;
using Adnc.Infr.Common.Exceptions;
using Adnc.Core.Shared.Domain.Entities;

namespace Adnc.Warehouse.Domain.Entities
{
    public class Product : AggregateRoot
    {
        public string Sku { private set; get; }

        public string Name { private set; get; }

        public string Describe { set; get; }

        public float Price { private set; get; }

        public ProductStatus Status { internal set; get; }

        public long? ShlefId { private set; get; }

        public string Unit {private set; get; }

        protected Product()
        {
        }

        /// <summary>
        /// 创建商品需要依赖仓储判断是否存在同名，所以必须要在领域服务类处理部分业务逻辑
        /// internal可以防止应用服务直接使用Product的构造函数去创建实例,限制必须使用ProductManager来创建.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sku"></param>
        /// <param name="name"></param>
        /// <param name="unit"></param>
        /// <param name="describe"></param>
        internal Product(long id, string sku,float price, string name,string unit, string describe = null)
        {
            this.Id = id;
            SetSku(sku);
            SetName(name);
            SetPrice(price);
            SetUnit(unit);
            this.Describe = describe ?? string.Empty;
            this.Status = new ProductStatus(ProductStatusEnum.UnKnow, string.Empty);
        }



        /// <summary>
        /// 设置unit
        /// </summary>
        /// <param name="newSku"></param>
        public void SetUnit(string unit)
        {
            this.Unit = Checker.NotNullOrEmpty(unit.Trim(), nameof(unit));
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
            this.Name = Checker.NotNullOrEmpty(name.Trim(),nameof(name));
        }

        /// <summary>
        /// 设置Price
        /// </summary>
        /// <param name="price"></param>
        public void SetPrice(float price)
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

        /// <summary>
        /// 设置仓库的货架Id
        /// </summary>
        /// <param name="shelfId"></param>
        public void SetShelf(long shelfId)
        {
            if (this.ShlefId == shelfId)
                return;

            this.ShlefId = Checker.GTZero(shelfId, nameof(shelfId));
        }
    }
}
