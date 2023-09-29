using Core.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;
using WebShop.Authorization.Constants;

namespace WebShop.Controllers
{
    [Route("api")]
    [ApiController]
    public class ProductsController : WebShopBaseController
    {
        private readonly IProductsService _productService;

        public ProductsController(IProductsService productService, IUsersService usersService) 
            : base(usersService)
        {
            _productService = productService;
        }

        [HttpGet("products")]
        public async Task<List<ProductViewModel>> GetAllProducts()
        {
            return await _productService.GetAllProductsAsync();
        }

        [HttpPost("products")]
        [Authorize(Policy = AuthorizationPolicies.RequireAdminPolicy)]
        public async Task<IActionResult> Insert([FromBody] ProductViewModel productModel)
        {
            await _productService.InsertAsync(productModel);
            return Ok();
        }

        [HttpGet("products/search/{keyword}")]
        public async Task<List<ProductViewModel>> SearchByKeyword(string keyword)
        {
            return await _productService.SearchByKeyWordAsync(keyword);
        }

        [HttpGet("products/{productId}")]
        public async Task<ProductViewModel> GetById(int productId)
        {
            return await _productService.GetByIdAsync(productId);
        }

        [HttpDelete("products/{productId}")]
        [Authorize(Policy = AuthorizationPolicies.RequireAdminPolicy)]
        public async Task<bool> DeleteById(int productId)
        {
            return await _productService.DeleteAsync(productId);
        }

        [HttpPut("products")]
        [Authorize(Policy = AuthorizationPolicies.RequireAdminPolicy)]
        public async Task<bool> UpdateProducts(int productId, ProductViewModel productViewModel)
        {
            return await _productService.UpdateAsync(productId, productViewModel);
        }
    }
}