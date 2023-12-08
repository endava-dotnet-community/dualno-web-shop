using Core.Abstractions.Repositories;
using DatabaseEF.Entities;
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
        public async Task<bool> DeleteShoppingCartAsync(long cartId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteShoppingCartItemAsync(long cartItemId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ShoppingCart>> GetAllShoppingCartsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ShoppingCart> GetBySessionIdAsync(string sessionId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> InsertShoppingCartAsync(ShoppingCart shoppingCart)//-------------------------------
        {
            if (shoppingCart == null)
                return false;

            ShoppingCartEntity entity = MapToEntity(shoppingCart);
            await _context.Cart.AddAsync(entity);
            await _context.SaveChangesAsync();

            return true;
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
        private static ShoppingCartEntity MapToEntity(ShoppingCart p)
        {
            if (p == null)
                return null;

            return new ShoppingCartEntity
            {
                Id = p.Id,
                AccessedAt = p.AccessedAt,
                SessionId = p.SessionId,
            };
        }
    }
}
