using Core.Abstractions.Repositories;
using DatabaseEF.Entities;
using Domain;
using System.Data.Entity;
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
            ShoppingCartEntity entity = await _context.Carts.FindAsync(cartId);

            if (entity == null)
                return false;

            _context.Carts.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteShoppingCartItemAsync(long cartItemId) 
        {
            throw new NotImplementedException();
        }

        public async Task<List<ShoppingCart>> GetAllShoppingCartsAsync()
        {
            return _context
                .Carts
                .AsNoTracking()
                .Select(e => MapFromEntity(e))
                .ToList();
        }

        private static ShoppingCartItem MapFromEntity(ShoppingCartItemEntity sci)
        {
            if (sci == null)
                return null;

            return new ShoppingCartItem
            {
                Id = sci.Id,
                ProductId = sci.Product.Id,
                Quantity = sci.Quantity
            };
        }

        private static ShoppingCart MapFromEntity(ShoppingCartEntity sc)
        {
            if (sc == null)
                return null;

            return new ShoppingCart
            {
                Id = sc.Id,
                AccessedAt = sc.AccessedAt,
                SessionId = sc.SessionId,
                Items = sc.Items.Select(i => MapFromEntity(i)).ToList()
            };
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
