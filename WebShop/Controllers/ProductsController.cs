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
        public List<ProductViewModel> GetAllProducts()
        {
            return _productService.GetAllProducts();
        }

        [HttpPost("products")]
        [Authorize(Policy = AuthorizationPolicies.RequireAdminPolicy)]
        public IActionResult Insert([FromBody] ProductViewModel productModel)
        {
            _productService.Insert(productModel);
            return Ok();
        }

        [HttpGet("products/search/{keyword}")]
        public List<ProductViewModel> SearchByKeyword(string keyword)
        {
            return _productService.SearchByKeyWord(keyword);
        }

        [HttpGet("products/{productId}")]
        public ProductViewModel GetById(int productId)
        {
            return _productService.GetById(productId);
        }

        [HttpDelete("products/{productId}")]
        [Authorize(Policy = AuthorizationPolicies.RequireAdminPolicy)]
        public bool DeleteById(int productId)
        {
            return _productService.Delete(productId);
        }

        [HttpPut("products")]
        [Authorize(Policy = AuthorizationPolicies.RequireAdminPolicy)]
        public bool UpdateProducts(int productId, ProductViewModel productViewModel)
        {
            return _productService.Update(productId, productViewModel);
        }
    }
}