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
            throw new NotImplementedException();
        }

        public async Task<ShoppingCart> GetBySessionIdAsync(string sessionId)
        {
            ShoppingCartEntity entity = (await _context.ShoppingCarts.FindAsync(sessionId));
            return MapFromEntity(entity);
        }

        public async Task<bool> InsertShoppingCartAsync(ShoppingCart shoppingCart) 
        {
            throw new NotImplementedException();
        }

        public async Task<bool> InsertShoppingCartItemAsync(ShoppingCartItem shoppingCartItem) 
        {
            if (shoppingCartItem == null)
                return false;

            ShoppingCartItemEntity entity = MapToEntity(shoppingCartItem);
            await _context.CartItems.AddAsync(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAccessedAtAsync(long cartId, DateTime accessedAt) 
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateQuantityAsync(long cartItemId, int quantity)
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
                Items = p.Items.Select(i => MapFromEntityItems(i)).ToList(),

            };
        }

        private static ShoppingCartItemEntity MapToEntity(ShoppingCartItem p)
        {
            if (p == null)
                return null;
            return new ShoppingCartItemEntity
            {
                Id = p.Id,
                Product = new ProductEntity
                {
                    Id = p.ProductId
                },
                Quantity = p.Quantity
            };
        }

    }
}
