using Core.Abstractions.Services;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;

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
        public List<ProductViewModel?> GetAllProducts()
        {
            return _productService.GetAllProducts();
        }

        [HttpPost("products")]
        public IActionResult Insert([FromBody] ProductViewModel productModel)
        {
            if (!CurrentUser.Roles.Contains(UserRole.Administrator))
            {
                throw new InvalidOperationException();
            }

            try
            {
                _productService.Insert(productModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpGet("products/search/{keyword}")]
        public List<ProductViewModel?> SearchByKeyword(string keyword)
        {
            return _productService.SearchByKeyWord(keyword);
        }

        [HttpGet("products/{productId}")]
        public ProductViewModel? GetById(int productId)
        {
            return _productService.GetById(productId);
        }

        [HttpDelete("products/{productId}")]
        public bool DeleteById(int productId)
        {
            if (!CurrentUser.Roles.Contains(UserRole.Administrator))
            {
                throw new InvalidOperationException();
            }

            return _productService.Delete(productId);
        }

        [HttpPut("products")]
        public bool UpdateProducts(int productId, ProductViewModel productViewModel)
        {
            if (!CurrentUser.Roles.Contains(UserRole.Administrator))
            {
                throw new InvalidOperationException();
            }

            return _productService.Update(productId, productViewModel);
        }
    }
}