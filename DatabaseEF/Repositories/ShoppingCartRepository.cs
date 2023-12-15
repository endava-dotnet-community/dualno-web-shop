using Core.Abstractions.Repositories;
using DatabaseEF.Entities;
using DatabaseEF.Migrations;
using Domain;
using WebShop.DatabaseEF.Entities;

namespace DatabaseEF.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        readonly WebshopContext _context;
        public ShoppingCartRepository(WebshopContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<bool> DeleteShoppingCartAsync(long cartId) { 
            return true;
        }

        public async Task<bool> DeleteShoppingCartItemAsync(long cartItemId) 
        {
            ShoppingCartItemEntity entity = await _context.CartItems.FindAsync(cartItemId);

            if (entity == null)
                return false;

            _context.CartItems.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<ShoppingCart>> GetAllShoppingCartsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ShoppingCart> GetBySessionIdAsync(string sessionId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> InsertShoppingCartAsync(ShoppingCart shoppingCart)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> InsertShoppingCartItemAsync(ShoppingCartItem shoppingCartItem)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAccessedAtAsync(long cartId, DateTime accessedAt)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateQuantityAsync(long cartItemId, int quantity)
        {
            throw new NotImplementedException();
        }
    }
}
