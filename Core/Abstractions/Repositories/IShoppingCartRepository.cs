using Domain;

namespace Core.Abstractions.Repositories
{
    public interface IShoppingCartRepository
    {
        Task<ShoppingCart> GetBySessionIdAsync(string sessionId);
        Task<List<ShoppingCart>> GetAllShoppingCartsAsync();
        Task<bool> InsertShoppingCartAsync(ShoppingCart shoppingCart);
        Task<bool> UpdateAccessedAtAsync(long cartId, DateTime accessedAt);
        Task<bool> DeleteShoppingCartAsync(long cartId);
        Task<bool> InsertShoppingCartItemAsync(ShoppingCartItem shoppingCartItem);
        Task<bool> UpdateQuantityAsync(long cartItemId, int quantity);
        Task<bool> DeleteShoppingCartItemAsync(long cartItemId);
    }
}
