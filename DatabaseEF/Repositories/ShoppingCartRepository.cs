using Core.Abstractions.Repositories;
using DatabaseEF.Entities;
using Domain;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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

        public async Task<ShoppingCart> GetBySessionIdAsync(string sessionId)
        {
            ShoppingCartEntity entity = (await _context.ShoppingCarts.FindAsync(sessionId));
            return MapFromEntity(entity);
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
        private static ShoppingCartItem MapFromEntityItems (ShoppingCartItemEntity p)
        {
            if (p == null)
                return null;
            return new ShoppingCartItem
            {
                Id = p.Id,
                ProductId = p.Product.Id,
                Quantity = p.Quantity,
            };
        }
        private static ShoppingCart MapFromEntity(ShoppingCartEntity p)
        {
            if (p == null)
                return null;

            return new ShoppingCart
            {
            
                Id = p.Id,
                AccessedAt = p.AccessedAt,
                SessionId = p.SessionId,
                Items = p.Items.Select(i =>MapFromEntityItems(i)).ToList(),
                    
            };
        }

    }
}
