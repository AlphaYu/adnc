using System;
using System.Threading.Tasks;
using Adnc.Core.Shared;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infr.Common.Exceptions;
using Adnc.Infr.Common.Helper;
using Adnc.Whse.Domain.Entities;

namespace Adnc.Whse.Domain.Services
{
    public class ProductManager : ICoreService
    {
        private readonly IEfBasicRepository<Product> _productRepo;
        public ProductManager(IEfBasicRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        /// <summary>
        /// 创建商品
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="name"></param>
        /// <param name="unit"></param>
        /// <param name="describe"></param>
        /// <returns></returns>
        public virtual async Task<Product> CreateAsync(string sku, decimal price, string name, string unit, string describe = null)
        {
            var product = await _productRepo.Where(x => x.Sku == sku || x.Name == name).FirstOrDefaultAsync();
            if (product != null)
            {
                if (product.Sku == sku)
                    throw new ArgumentException("sku exists");
                if (product.Name == name)
                    throw new ArgumentException("name exists");
            }

            return new Product(
                IdGenerater.GetNextId(IdGenerater.DatacenterId, IdGenerater.WorkerId)
                , sku
                , price
                , name
                , unit
                , describe
            );
        }

        /// <summary>
        /// 修改SKU
        /// </summary>
        /// <param name="product"></param>
        /// <param name="newSku"></param>
        /// <returns></returns>
        public virtual async Task ChangeSkuAsync(Product product, string newSku)
        {
            if (product.Sku == newSku)
                return;

            var exists = await _productRepo.AnyAsync(x => x.Sku == newSku);
            if (exists)
                throw new ArgumentException("newsku");

            product.SetSku(newSku);
        }

        /// <summary>
        /// 修改商品名称
        /// </summary>
        /// <param name="product"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        public virtual async Task ChangeNameAsync(Product product, string newName)
        {
            if (product.Name == newName)
                return;

            var exists = await _productRepo.AnyAsync(x => x.Name == newName);
            if (exists)
                throw new ArgumentException("newName");

            product.SetName(newName);
        }

        /// <summary>
        /// 上架商品
        /// </summary>
        /// <param name="product"></param>
        /// <param name="warehouseInfo"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public virtual void PutOnSale(Product product, Warehouse warehouseInfo, string reason)
        {
            Checker.NotNull(product, nameof(product));

            Checker.NotNull(warehouseInfo, nameof(warehouseInfo));

            if (warehouseInfo.Qty > 0 && warehouseInfo.ProductId == product.Id)
                product.SetStatus(new ProductStatus(ProductStatusEnum.SaleOn, reason));
            else
                throw new ArgumentException("warehouseInfo");
        }
    }
}
