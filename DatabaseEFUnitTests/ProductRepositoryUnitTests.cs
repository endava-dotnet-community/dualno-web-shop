using DatabaseEF.Repositories;
using Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using WebShop.DatabaseEF.Entities;

namespace DatabaseEFUnitTests
{
    [TestClass]
    public class ProductRepositoryUnitTests : DatabaseUnitTests
    {
        [TestMethod]
        public async Task GetByIdAsyncTestMethod()
        {
            using var context = CreateDbContext();
            var repository = new ProductRepository(context);
            await repository.InsertAsync(
                new Domain.Product
                {
                    Name = "Test1",
                    CategoryId = 1,
                    Description = "Description1",
                    Price = 100
                });
            var result = await repository.GetByIdAsync(1);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }


        [TestMethod]
        public async Task DeleteAsyncTestMethod()
        {
            using var context = CreateDbContext();
            var repository = new ProductRepository(context);
            await repository.InsertAsync(
                new Domain.Product
                {
                    Name = "test1",
                    Price = 200,
                    CategoryId = 1,
                    Description = "test1",
                }
                );
            var result = await repository.DeleteAsync(1);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public async Task DeleteAsyncNotFoundTestMethod()
        {
            using var context = CreateDbContext();
            var repository = new ProductRepository(context);
            var result = await repository.DeleteAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result);
        }


        [TestMethod]
        public async Task InsertAsyncTestMethod()
        {
            using var context = CreateDbContext();
            var repository = new ProductRepository(context);
            var newproduct = new Product
            {
                Name = "Test1",
                CategoryId = 1,
                Description = "Description1",
                Price = 600
            };
            var result = await repository.InsertAsync(newproduct);
            var result2 = await repository.GetByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.IsTrue(result);
            Assert.AreEqual(1, result2.Id);
        }

        [TestMethod]
        public async Task InsertNullAsyncTestMethod()
        {
            using var context = CreateDbContext();
            var repository = new ProductRepository(context);
            var result = await repository.InsertAsync(null);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public async Task UpdateAsyncTestMethod()
        {
            using var context = CreateDbContext();
            var repository = new ProductRepository(context);
            var product1 = new Product
            {
                Id = 1,
                CategoryId = 1,
                Price = 1,
                Description = null,
                Name = null,
            };
            var product2 = new Product
            {
                CategoryId = 2,
                Price = 2,
                Description = null,
                Name = "Product2"
            };

            var insertResult = await repository.InsertAsync(product1);

            Assert.IsNotNull(insertResult);
            Assert.IsTrue(insertResult);

            var updateResult = await repository.UpdateAsync(1, product2);

            Assert.IsNotNull(updateResult);
            Assert.IsTrue(updateResult);

            var product = await repository.GetByIdAsync(1);

            Assert.IsNotNull(product);
            Assert.AreEqual(2, product.CategoryId);
            Assert.AreEqual(2, product.Price);
            Assert.IsNull(product.Description);
            Assert.AreEqual("Product2", product.Name);
        }

        [TestMethod]
        public async Task UpdateNotFoundAsyncTestMethod()
        {
            using var context = CreateDbContext();
            var repository = new ProductRepository(context);

            var result = await repository.UpdateAsync(1, new Product());

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public async Task SearchByKeyWordTestMethod()
        {
            const string keyWord = "name";
            using var context = CreateDbContext();
            var repo = new ProductRepository(context);
            await repo.InsertAsync(
              new Domain.Product
              {
                  Name = "Name1",
                  Description = "Desc1",
                  Price = 300,
                  CategoryId = 1,
              });
            var result = await repo.SearchByKeyWordAsync(keyWord);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1);
        }

        [TestMethod]
        public async Task SearchByKeyWordTestMethod1()
        {
            const string keyWord = "dESc";
            using var context = CreateDbContext();
            var repo = new ProductRepository(context);
            await repo.InsertAsync(
              new Domain.Product
              {
                  Name = "Name2",
                  Description = "this is DESCRiption",
                  Price = 300,
                  CategoryId = 2
              });
            var result = await repo.SearchByKeyWordAsync(keyWord);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetAllProductsTestMethod()
        {
            using var context = CreateDbContext();
            var repository = new ProductRepository(context);
            await repository.InsertAsync
                (new Domain.Product { CategoryId = 1, Description = "Opis1", Name = "Kafica2", Price = 150 });
            await repository.InsertAsync
                (new Domain.Product { CategoryId = 2, Description = "Opis2", Name = "Kafica2", Price = 200 });
            await repository.InsertAsync
                (new Domain.Product { CategoryId = 3, Description = "Opis3", Name = "Kafica3", Price = 350 });
            var result = await repository.GetAllProductsAsync();
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(2, result[1].Id);
            Assert.IsInstanceOfType(result[0], typeof(Domain.Product));
        }
    }
}