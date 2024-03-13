using Core.Abstractions.Repositories;
using DatabaseEF.Entities;
using DatabaseEF.Migrations;
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
            ShoppingCartItemEntity entity = await _context.CartItems.FindAsync(cartItemId);

            if (entity == null)
                return false;

            _context.CartItems.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<ShoppingCart>> GetAllShoppingCartsAsync()
        {
            return _context
                .Carts
                .Include(c => c.Items)
                .AsNoTracking()
                .Select(e => MapFromEntity(e))
                .ToList();
        }

        public async Task<ShoppingCart> GetBySessionIdAsync(string sessionId)
        {
            ShoppingCartEntity entity = _context.Carts.Include(c => c.Items).FirstOrDefault(c => c.SessionId == sessionId);
            return MapFromEntity(entity);
        }

        public async Task<bool> InsertShoppingCartAsync(ShoppingCart shoppingCart)
        {
            if (shoppingCart == null)
                return false;

            ShoppingCartEntity entity = MapToEntity(shoppingCart);
            await _context.Carts.AddAsync(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> InsertShoppingCartItemAsync(ShoppingCartItem shoppingCartItem) 
        {
            if (shoppingCartItem == null)
                return false;

            ShoppingCartItemEntity entity = MapToEntityItem(shoppingCartItem);

            if (_context.CartItems.ToList().Any(c => c.ProductId == entity.ProductId && c.CartId == entity.CartId))
                return false;

            await _context.CartItems.AddAsync(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAccessedAtAsync(long cartId, DateTime accessedAt) 
        {
            ShoppingCartEntity entity = await _context.Carts.FindAsync(cartId);
            if (entity == null)
                return false;

            entity.AccessedAt = accessedAt;
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> UpdateQuantityAsync(long cartItemId, int quantity)
        {
            if(quantity<0)
                return false;

            ShoppingCartItemEntity entity = await _context.CartItems.FindAsync(cartItemId);
            if (entity == null)
                return false;

            entity.Quantity = quantity;
            await UpdateAccessedAtAsync(entity.CartId, DateTime.UtcNow);

            return true;
        }

        public async Task<List<ShoppingCartItem>> GetAllShoppingCartItemsAsync(long cartId)
        {
            ShoppingCartEntity entity = await _context.Carts.FindAsync(cartId);

            if (entity == null) return null;

            var result = entity.Items.Select(p => MapFromEntityItems(p)).ToList();

            return result;
        }

        private static ShoppingCartItem MapFromEntityItems (ShoppingCartItemEntity p)
        {
            if (p == null)
                return null;

            return new ShoppingCartItem
            {
                Id = p.Id,
                CartId = p.CartId,
                ProductId = p.ProductId,
                Quantity = p.Quantity
            };
        }

        private static ShoppingCartItemEntity MapToEntityItem(ShoppingCartItem p)
        {
            if (p == null)
                return null;

            return new ShoppingCartItemEntity
            {
                Id = p.Id,
                ProductId = p.ProductId,
                Quantity = p.Quantity,
                CartId = p.CartId
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
                Items = p.Items.Select(i => MapFromEntityItems(i)).ToList(),
            };
        }

        private static ShoppingCartEntity MapToEntity(ShoppingCart p)
        {
            if (p == null)
                return null;

            return new ShoppingCartEntity
            {
                Id = p.Id,
                SessionId = p.SessionId,
                AccessedAt = p.AccessedAt,
                Items = p.Items == null ? new() : p.Items.Select(p => MapToEntityItem(p)).ToList()
            };
        }
    }
}
