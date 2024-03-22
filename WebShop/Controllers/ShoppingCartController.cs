using Core.Abstractions.Services;
using Domain;
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
        //Add Product to shoppingcart(product.Id)   //id cartId productId, quantity  product = shoppingCartItem
        //Delete Product to shoppingcart(product.Id)
        //Update Quntity (product.Id)
        //Checkout


        /* CartItem -> productId, cartId, quantitiy, Id
         * 
         * 
         * */

        [HttpGet("Add")]
        public async Task<bool> AddProduct(long productId)
        {
            try
            {
                var shoppingCart = await _shoppingCartService.GetBySessionIdAsync(HttpContext.Session.Id);
                var cartItem = new ShoppingCartItemViewModel
                {
                    ProductId = productId,
                    Quantity = 1
                };

                var result = await _shoppingCartService.InsertShoppingCartItemAsync(HttpContext.Session.Id, cartItem);

                return result;
            }
            catch
            {
                return false;
            }
        }

        [HttpDelete("Delete")]
        public async Task<bool> DeleteProduct(long productId)
        {
            try
            {
                var result = await _shoppingCartService.DeleteShoppingCartItemAsync(productId);
                return result;
            }
            catch
            {
                return false; 

            }
        }
        [HttpPut("Update")]
        public async Task<bool> UpdataQuantity(long productId, int newQuantity)
        {
            try
            {
                var result = await _shoppingCartService.UpdateQuantityAsync(productId, newQuantity);
                return result;
            }
            catch
            {
                return false;
            }
        }

        [HttpGet("Checkout")]
        
        public async Task<List<ShoppingCartItemViewModel>> Checkout()
        {
            try
            {
                var shoppingCart = await _shoppingCartService.GetBySessionIdAsync(HttpContext.Session.Id);
                var result = await _shoppingCartService.GetAllShoppingCartItemsAsync(shoppingCart.Id);

                return result;

            }
            catch
            {
                return null;
            }
        }



    }
}