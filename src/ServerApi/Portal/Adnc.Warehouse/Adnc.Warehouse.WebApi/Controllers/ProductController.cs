using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Adnc.Warehouse.Application.Services;
using Adnc.Warehouse.Application.Dtos;
using Adnc.WebApi.Shared;
using Adnc.Infr.Common.Extensions;
using System.Collections.Generic;
using Adnc.Application.Shared.Dtos;

namespace Adnc.Warehouse.WebApi.Controllers
{
    /// <summary>
    /// 商品管理
    /// </summary>
    [Route("warehouse/products")]
    [ApiController]
    public class ProductController : AdncControllerBase
    {
        private readonly IProductAppService _productSrv;

        public ProductController(IProductAppService productSrv)
        {
            _productSrv = productSrv;
        }

        /// <summary>
        /// 新建商品
        /// </summary>
        /// <param name="input"><see cref="ProductCreationDto"/></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateAsync([FromBody] ProductCreationDto input)
        {
            return await _productSrv.CreateAsync(input);
        }

        /// <summary>
        /// 更新商品
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> UpdateAsync([FromRoute] string id, [FromBody] ProductUpdationDto input)
        {
            var productId = id.ToLong();
            return await _productSrv.UpdateAsync(productId.Value, input);
        }

        /// <summary>
        /// 调整价格
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPatch("{id}/price")]
        public async Task<ActionResult<ProductDto>> ChangePriceAsync([FromRoute] string id, ProducChangePriceDto input)
        {
            var productId = id.ToLong();
            return await _productSrv.ChangePriceAsync(productId.Value, input);
        }

        /// <summary>
        /// 上架商品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPatch("{id}/status/1001")]
        public async Task<ActionResult<ProductDto>> PutOnSaleAsync([FromRoute] string id, ProductPutOnSaleDto input)
        {
            var productId = id.ToLong();
            return await _productSrv.PutOnSaleAsync(productId.Value, input);
        }

        /// <summary>
        /// 下架商品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPatch("{id}/status/1002")]
        public async Task<ActionResult<ProductDto>> PutOffSaleAsync([FromRoute] string id, ProductPutOffSaleDto input)
        {
            var productId = id.ToLong();
            return await _productSrv.PutOffSaleAsync(productId.Value, input);
        }

        /// <summary>
        /// 商品列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetListAsync([FromQuery] ProductSearchListDto search)
        {
            return await _productSrv.GetListAsync(search);
        }


        /// <summary>
        /// 商品分页列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<PageModelDto<ProductDto>>> GetPagedAsync([FromQuery] ProductSearchPagedDto search)
        {
            return await _productSrv.GetPagedAsync(search);
        }
    }
}