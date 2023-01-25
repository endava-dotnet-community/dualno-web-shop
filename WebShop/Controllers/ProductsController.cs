using Core.Abstractions.Services;
using Core.Domain;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;

namespace WebShop.Controllers
{
    [Route("api")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productService;

        public ProductsController(IProductsService productService)
        {
            _productService = productService;
        }

        [HttpGet("products")]
        public List<Proizvod> GetAllProducts()
        {
            List<Proizvod> proizvodi = _productService.GetAllProducts();

            return proizvodi;
        }

        [HttpPost("products")]
        public IActionResult Insert([FromBody] ProizvodModel productModel)
        {
            var p = new Proizvod
            {
                Cena = productModel.Cena,
                Ime = productModel.Ime,
                Kategorija = productModel.Kategorija,
                Opis = productModel.Opis
            };

            _productService.InsertProduct(p);

            return Ok();
        }
    }
}