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
        public async Task GetAllShoppingCartsAsyncTestMethod()
        {
            //veljko
            var context = CreateDbContext();
            var shoppingCartRepository = new ShoppingCartRepository(context);

            await CreateShoppingCart(shoppingCartRepository, "1", DateTime.Now);
            await CreateShoppingCart(shoppingCartRepository, "2", DateTime.Now);
            await CreateShoppingCart(shoppingCartRepository, "3", DateTime.Now);

            var result = await shoppingCartRepository.GetAllShoppingCartsAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(2, result[1].Id);
        }

        [TestMethod]
        public async Task InsertShoppingCartAsyncTestMethod(/*ShoppingCart shoppingCart*/)
        {
            // lazar
            var sessionId = Guid.NewGuid().ToString();
            var accessedAt = DateTime.UtcNow;
            using var context = CreateDbContext();
            var repository = new ShoppingCartRepository(context);
            await CreateShoppingCart(repository, sessionId, accessedAt);
            var result = await repository.GetBySessionIdAsync(sessionId);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        public async Task UpdateAccessedAtAsyncTestMethod()
        {
            // lara
            using var context = CreateDbContext();
            var repository = new ShoppingCartRepository(context);
            DateTime dt1 = new DateTime(2024, 1, 1);
            await CreateShoppingCart(repository, "1", dt1);
            DateTime dt2 = new DateTime(2024, 1, 2);
            var result = repository.UpdateAccessedAtAsync(1, dt2);
            var cart = await repository.GetBySessionIdAsync("1");
            var dataTime = cart.AccessedAt;
            Assert.IsNotNull(result);
            Assert.AreEqual(dataTime, dt2);
        }

        [TestMethod]
        public async Task UpdateAccessedAtNotFoundTestMethod()
        {
            using var context = CreateDbContext();
            var repository = new ShoppingCartRepository(context);
            var result = await repository.UpdateAccessedAtAsync(1, DateTime.Now);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public async Task InsertShoppingCartNotGivenAsyncTestMethod()
        {
            using var context = CreateDbContext();
            var repository = new ShoppingCartRepository(context);
            var result = await repository.InsertShoppingCartAsync(null);
            
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public async Task DeleteShoppingCartAsyncTestMethod()
        {
            using var context = CreateDbContext();
            var repository = new ShoppingCartRepository(context);
            await repository.InsertShoppingCartAsync(
                new Domain.ShoppingCart
                {
                    Items = new List<ShoppingCartItem>()
                });
            var result = await repository.DeleteShoppingCartAsync(1);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public async Task DeleteShoppingCartNotFoundAsyncTestMethod()
        {
            using var context = CreateDbContext();
            var repository = new ShoppingCartRepository(context);

            var result = await repository.DeleteShoppingCartAsync(1);
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public async Task InsertShoppingCartItemAsyncTestMethod()
        {
            // mladen
            using var context = CreateDbContext();
            var repository = new ShoppingCartRepository(context);

            await CreateShoppingCart(repository, "1", DateTime.Now);
            await CreateShoppingCartItems(repository, 1);
            var shoppingCart = await repository.GetBySessionIdAsync("1");

            Assert.IsNotNull(shoppingCart.Items);
            Assert.AreEqual(shoppingCart.Items.Count, 2);
        }

        [TestMethod]
        public async Task InsertNoShoppingCartItemAsyncTestMethod()
        {
            using var context = CreateDbContext();
            var repository = new ShoppingCartRepository(context);

            var result = await repository.InsertShoppingCartItemAsync(null);
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public async Task UpdateQuantityAsyncTestMethod()
        {
            // mihajlo negativna kolicina
            var sessionId = Guid.NewGuid().ToString();
            var accessedAt = DateTime.UtcNow;
            using var context = CreateDbContext();
            await CreateTestData(context, sessionId, accessedAt);

            var repository = new ShoppingCartRepository(context);

            var result = await repository.UpdateQuantityAsync(1, 2);
            var shoppingCart = await repository.GetAllShoppingCartsAsync();//puca zbog vincica
            Assert.IsTrue(result);
            Assert.AreEqual(shoppingCart[0].Items[0].Quantity, 2);
        }

        [TestMethod]
        public async Task UpdateQuantityAsyncNotFoundTestMethod()
        {
            using var context = CreateDbContext();
            var repository = new ShoppingCartRepository(context);

            var result = await repository.UpdateQuantityAsync(1, 2);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public async Task UpdateQuantityAsyncForNegativeQuanityTestMethod()
        {
            var sessionId = Guid.NewGuid().ToString();
            var accessedAt = DateTime.UtcNow;
            using var context = CreateDbContext();
            await CreateTestData(context, sessionId, accessedAt);

            var repository = new ShoppingCartRepository(context);

            var result = await repository.UpdateQuantityAsync(1, -1);
            Assert.AreEqual(false, result);

            var shoppingCart = await repository.GetBySessionIdAsync(sessionId);
            var item = shoppingCart.Items.Find(i => i.Id == 1);
            Assert.AreEqual(1, item.Quantity);
        }

        [TestMethod]
        public async Task DeleteShoppingCartItemAsyncTestMethod()
        {
            //david
            var sessionId = Guid.NewGuid().ToString();
            var accessedAt = DateTime.UtcNow;
            using var context = CreateDbContext();
            await CreateTestData(context, sessionId, accessedAt);

            var repository = new ShoppingCartRepository(context);

            var result = await repository.DeleteShoppingCartItemAsync(1);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);

            var result2 = await repository.GetBySessionIdAsync(sessionId);
            Assert.AreEqual(false, result2.Items.Any(c => c.Id==1));

        }

        [TestMethod]
        public async Task DeleteShoppingCartItemNotFoundAsyncTestMethod()
        {
            //david
            
            using var context = CreateDbContext();

            var repository = new ShoppingCartRepository(context);

            var result = await repository.DeleteShoppingCartItemAsync(1);
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public async Task InsertExistingItemAsyncTestMethod()
        {
            var sessionId = Guid.NewGuid().ToString();
            var accessedAt = DateTime.UtcNow;
            using var context = CreateDbContext();
            await CreateTestData(context, sessionId, accessedAt);

            var repository = new ShoppingCartRepository(context);
            var newShoppingCartItem = new ShoppingCartItem
            {
                ProductId = 1,
                CartId = 1,
                Quantity = 1
            };
            var result = await repository.InsertShoppingCartItemAsync(newShoppingCartItem);
            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }
    }
}