using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Adnc.Warehouse.Core.Entities
{
    public class Product : AggregateRoot<long>
    {
        public string Sku { private set; get; }

        public string Name { private set; get; }

        public string Describe { set; get; }

        public float Price { private set; get; }

        public ProductStatus Status { internal set; get; }

        public long? AssignedWarehouseId { private set; get; }

        public string Unit {set; get; }

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
            this.ID = id;
            this.Sku = sku.Trim();
            this.Name = name.Trim();
            this.Unit = unit.Trim();
            this.Price = price;
            this.Describe = describe ?? string.Empty;
            this.Status = new ProductStatus(ProductStatusEnum.UnKnow, string.Empty);
        }

        /// <summary>
        /// 设置sku
        /// </summary>
        /// <param name="newSku"></param>
        internal void SetSku(string newSku)
        {
            if(string.IsNullOrWhiteSpace(newSku))
                throw new ArgumentException("newSku");

            this.Sku = newSku;
        }


        /// <summary>
        /// 设置Name
        /// </summary>
        /// <param name="newName"></param>
        internal void SetName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("newName");

            this.Name = newName;
        }

        /// <summary>
        /// 设置Price
        /// </summary>
        /// <param name="newPrice"></param>
        public void SetPrice(float newPrice)
        {
            if (newPrice <= 0)
                throw new ArgumentException("newPrice");

            this.Price = newPrice;
        }

        /// <summary>
        /// 下架商品
        /// </summary>
        public void PutOffSale(string reason)
        {
            this.Status = new ProductStatus(ProductStatusEnum.SaleOff, reason);
        }
    }
}
