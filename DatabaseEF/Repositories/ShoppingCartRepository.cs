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
        public async Task<bool> DeleteShoppingCartAsync(long cartId) // ja
        {
            ShoppingCartEntity entity = await _context.Carts.FindAsync(cartId);

            if (entity == null)
                return false;

            _context.Carts.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteShoppingCartItemAsync(long cartItemId) // david
        {
            throw new NotImplementedException();
        }

        public async Task<List<ShoppingCart>> GetAllShoppingCartsAsync()// lara
        {
            throw new NotImplementedException();
        }

        public async Task<ShoppingCart> GetBySessionIdAsync(string sessionId)// lara
        {
            throw new NotImplementedException();
        }

        public async Task<bool> InsertShoppingCartAsync(ShoppingCart shoppingCart) // mladen
        {
            throw new NotImplementedException();
        }

        public async Task<bool> InsertShoppingCartItemAsync(ShoppingCartItem shoppingCartItem) // ilija
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAccessedAtAsync(long cartId, DateTime accessedAt) // petar
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateQuantityAsync(long cartItemId, int quantity) // nikola
        {
            throw new NotImplementedException();
        }
    }
}
