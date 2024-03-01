using Core.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;
using WebShop.Authorization.Constants;

namespace WebShop.Controllers
{
    [Route("api")]
    [ApiController]
    public class ShoppingCartController : WebShopBaseController
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService, IUsersService usersService) 
            : base(usersService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpGet("cart")]
        public async Task<ShoppingCartViewModel> GetShoppingCart()
        {
            try
            {
                var shoppingCart = await _shoppingCartService.GetBySessionIdAsync(HttpContext.Session.Id);
                return shoppingCart;
            }
            catch
            {
                return null;
            }
        }
        //Add Product to shoppingcart(product.Id)
        //Delete Product to shoppingcart(product.Id)
        //Update Quntity (product.Id)
        //Checkout
        [HttpGet("Add")]
        public async Task<bool> AddProduct(long productId)
        {
            try
            {
                var result = await _shoppingCartService.InsertShoppingCartItemAsync(,)
            }
        }
    }
}