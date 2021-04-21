using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Adnc.WebApi.Shared;
using System.Collections.Generic;
using Adnc.Application.Shared.Dtos;
using Adnc.Whse.Application.Contracts.Dtos;
using Adnc.Whse.Application.Contracts.Services;

namespace Adnc.Whse.WebApi.Controllers
{
    /// <summary>
    /// 商品管理
    /// </summary>
    [Route("whse/products")]
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
        public async Task<ActionResult<ProductDto>> UpdateAsync([FromRoute] long id, [FromBody] ProductUpdationDto input)
        {
            return await _productSrv.UpdateAsync(id, input);
        }

        /// <summary>
        /// 调整价格
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPatch("{id}/price")]
        public async Task<ActionResult<ProductDto>> ChangePriceAsync([FromRoute] long id, ProducChangePriceDto input)
        {
            return await _productSrv.ChangePriceAsync(id, input);
        }

        /// <summary>
        /// 上架商品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPatch("{id}/status/1001")]
        public async Task<ActionResult<ProductDto>> PutOnSaleAsync([FromRoute] long id, ProductPutOnSaleDto input)
        {
            return await _productSrv.PutOnSaleAsync(id, input);
        }

        /// <summary>
        /// 下架商品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPatch("{id}/status/1002")]
        public async Task<ActionResult<ProductDto>> PutOffSaleAsync([FromRoute] long id, ProductPutOffSaleDto input)
        {
             return await _productSrv.PutOffSaleAsync(id, input);
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
        [HttpGet("page")]
        public async Task<ActionResult<PageModelDto<ProductDto>>> GetPagedAsync([FromQuery] ProductSearchPagedDto search)
        {
            return await _productSrv.GetPagedAsync(search);
        }
    }
}