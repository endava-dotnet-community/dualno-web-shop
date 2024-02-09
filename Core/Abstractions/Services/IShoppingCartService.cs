using Models;

namespace Core.Abstractions.Services
{
    public interface IShoppingCartService
    {
        Task<ShoppingCartViewModel> GetBySessionIdAsync(string sessionId);
        Task<List<ShoppingCartViewModel>> GetAllShoppingCartsAsync();
        Task<bool> InsertShoppingCartAsync(ShoppingCartViewModel shoppingCart);
        Task<bool> UpdateAccessedAtAsync(long cartId, DateTime accessedAt);
        Task<bool> DeleteShoppingCartAsync(long cartId);
        Task<bool> InsertShoppingCartItemAsync(ShoppingCartItemViewModel shoppingCartItem);
        Task<bool> UpdateQuantityAsync(long cartItemId, int quantity);
        Task<bool> DeleteShoppingCartItemAsync(long cartItemId);
    }
}
