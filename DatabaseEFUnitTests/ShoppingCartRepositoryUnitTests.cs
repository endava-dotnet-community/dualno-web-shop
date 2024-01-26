using DatabaseEF.Repositories;
using Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using System.Runtime.CompilerServices;
using WebShop.DatabaseEF.Entities;

namespace DatabaseEFUnitTests
{
    [TestClass]
    public class ShoppingCartRepositoryUnitTests : DatabaseUnitTests
    {
        private async Task CreateShoppingCart(ShoppingCartRepository repository, string sessionId, DateTime accessedAt)
        {
            await repository.InsertShoppingCartAsync(
                new Domain.ShoppingCart
                {
                    AccessedAt = accessedAt,
                    SessionId = sessionId,
                    Items = default
                });
        }

        private async Task CreateCategory(CategoryRepository repository)
        {
            await repository.InsertAsync(
                new Domain.Category
                {
                    Name = "Test1"
                });
        }

        private async Task CreateProduct(ProductRepository repository)
        {
            await repository.InsertAsync(
                new Domain.Product
                {
                    Name = "Product1",
                    CategoryId = 1,
                    Description = "Description1",
                    Price = 1000
                });

            await repository.InsertAsync(
                new Domain.Product
                {
                    Name = "Product2",
                    CategoryId = 1,
                    Description = "Description1",
                    Price = 10000
                });
        }

        private async Task CreateShoppingCartItems(ShoppingCartRepository repository, long cartId)
        {
               
            await repository.InsertShoppingCartItemAsync(
                new Domain.ShoppingCartItem
                {
                    ProductId = 1,
                    CartId = cartId,
                    Quantity = 1

                });

            await repository.InsertShoppingCartItemAsync(
                new Domain.ShoppingCartItem
                {
                    ProductId = 2,
                    CartId = cartId,
                    Quantity = 1

                });
        }

        private async Task CreateTestData(WebshopContext context, string sessionId, DateTime accessedAt)
        {
            var cartrepo = new ShoppingCartRepository(context);
            await CreateCategory(new CategoryRepository(context));
            await CreateProduct(new ProductRepository(context));
            await CreateShoppingCart(cartrepo, sessionId, accessedAt);
            await CreateShoppingCartItems(cartrepo, 1);

        }

        [TestMethod]
        public async Task GetBySessionIdAsyncTestMethod()
        {
            var sessionId = Guid.NewGuid().ToString();
            var accessedAt = DateTime.UtcNow;

            using var context = CreateDbContext();
            await CreateTestData(context, sessionId, accessedAt);

            var repository = new ShoppingCartRepository(context);
            var result = await repository.GetBySessionIdAsync(sessionId);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual(sessionId, result.SessionId);
            Assert.AreEqual(accessedAt, result.AccessedAt);
            Assert.AreEqual(2, result.Items.Count);

        }

        [TestMethod]
        public async Task<List<ShoppingCart>> GetAllShoppingCartsAsyncTestMethod()
        {
            // veljko
            throw new NotImplementedException();
        }

        [TestMethod]
        public async Task<bool> InsertShoppingCartAsyncTestMethod(ShoppingCart shoppingCart)
        {
            // lazar
            throw new NotImplementedException();
        }

        [TestMethod]
        public async Task<bool> UpdateAccessedAtAsyncTestMethod(long cartId, DateTime accessedAt)
        {
            // lara
            throw new NotImplementedException();
        }

        [TestMethod]
        public async Task<bool> DeleteShoppingCartAsyncTestMethod(long cartId)
        {
            // fica
            throw new NotImplementedException();
        }

        [TestMethod]
        public async Task<bool> InsertShoppingCartItemAsyncTestMethod(ShoppingCartItem shoppingCartItem)
        {
            // mladen
            throw new NotImplementedException();
        }

        [TestMethod]
        public async Task<bool> UpdateQuantityAsyncTestMethod(long cartItemId, int quantity)
        {
            // mihajlo
            throw new NotImplementedException();
        }

        [TestMethod]
        public async Task<bool> DeleteShoppingCartItemAsyncTestMethod(long cartItemId)
        {
            // david
            throw new NotImplementedException();
        }

    }
}