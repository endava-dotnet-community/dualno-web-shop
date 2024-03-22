using Domain;
using Models.ViewModels;

namespace Core.Abstractions.Services
{
    public interface IShoppingCartService
    {
        Task<ShoppingCartViewModel> GetBySessionIdAsync(string sessionId);
        Task<List<ShoppingCartViewModel>> GetAllShoppingCartsAsync();
        Task<bool> InsertShoppingCartAsync( ShoppingCartViewModel shoppingCart);
        Task<bool> DeleteShoppingCartAsync(long cartId);
        Task<bool> UpdateAccessedAtAsync(long cartId, DateTime accessedAt);
        Task<bool> InsertShoppingCartItemAsync(string sessionId, ShoppingCartItemViewModel shoppingCartItem);
        Task<bool> UpdateQuantityAsync(long cartItemId, int quantity);
        Task<bool> DeleteShoppingCartItemAsync(long cartItemId);
        Task<List<ShoppingCartItemViewModel>> GetAllShoppingCartItemsAsync(long cartId);
    }
}
