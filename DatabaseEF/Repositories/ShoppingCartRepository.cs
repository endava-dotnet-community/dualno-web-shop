using Core.Abstractions.Repositories;
using Domain;

namespace DatabaseEF.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        public Task<bool> DeleteShoppingCartAsync(long cartId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteShoppingCartItemAsync(long cartItemId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ShoppingCart>> GetAllShoppingCartsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ShoppingCart> GetBySessionIdAsync(string sessionId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertShoppingCartAsync(ShoppingCart shoppingCart)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertShoppingCartItemAsync(ShoppingCartItem shoppingCartItem)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAccessedAtAsync(long cartId, DateTime accessedAt)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateQuantityAsync(long cartItemId, int quantity)
        {
            throw new NotImplementedException();
        }
    }
}
