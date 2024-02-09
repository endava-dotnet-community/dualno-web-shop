using Core.Abstractions.Repositories;
using DatabaseEF.Repositories;
using Domain;
using FluentValidation;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Models.ViewModels;
using Moq;
using Services;
using System.Data.Common;
using WebShop.DatabaseEF.Entities;

namespace DatabaseEFUnitTests
{
    [TestClass]
    public class CategoryRepositoryUnitTests : DatabaseUnitTests
    {
        public List<Category> AllCategories { get; set; } = new List<Category>
        {
            new Category
            {
                Name = "Test1"
            },
            new Category
            {
                Name = "Test2"
            },
            new Category
            {
                Name = "Test3"
            }
        };

        [TestMethod]
        public async Task GetByIdAsyncTestMethod()
        {
            using var context = CreateDbContext();
            var repository = new CategoryRepository(context);

            await repository.InsertAsync(
                new Domain.Category
                {
                    Name = "Test1"
                });

            var result = await repository.GetByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        public async Task GetAllCategoriesAsyncTestMethod()
        {
            using var context = CreateDbContext();
            var repository = new CategoryRepository(context);

            AllCategories.ForEach(async c => await repository.InsertAsync(c));

            var result = repository.GetAllCategoriesAsync();

            Assert.AreEqual(3, result?.Result.Count);
        }

        [TestMethod]
        public async Task UpadteAsyncTestMethod()
        {
            using var context = CreateDbContext();
            var repository = new CategoryRepository(context);

            var data = new Category
            {
                Name = "Test1"
            };

            await repository.InsertAsync(data);
            data.Name = "Test2";

            var result = await repository.UpdateAsync(1, data);
            var category = await repository.GetByIdAsync(1);

            Assert.IsTrue(result);
            Assert.AreEqual("Test2", category.Name);
        }

        [TestMethod]
        public async Task UpdateNotFoundAsyncTestMethod()
        {
            using var context = CreateDbContext();
            var repository = new CategoryRepository(context);

            var result = await repository.UpdateAsync(1, new Category());

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public async Task InsertAsyncTestMethod()
        {
            using var context = CreateDbContext();
            var repository = new CategoryRepository(context);

            var newCategory = new Category
            {
                Name = "Test2"
            };

            var result = await repository.InsertAsync(newCategory);
            var Category = await repository.GetByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.IsTrue(result);
            Assert.AreEqual(1, Category.Id);
            Assert.AreEqual(newCategory.Name, Category.Name);
        }

        [TestMethod]
        public async Task InsertNullAsnycTestMethod()
        {
            using var context = CreateDbContext();
            var repository = new CategoryRepository(context);

            var result = await repository.InsertAsync(null);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public async Task DeleteAsyncTestMethod()
        {
            using var context = CreateDbContext();
            var repository = new CategoryRepository(context);

            await repository.InsertAsync(
                new Domain.Category
                {
                    Name = "Test2"
                });

            var result = await repository.DeleteAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public async Task DeleteNotFoundAsyncTestMethod()
        {
            var context = CreateDbContext();
            var repository = new CategoryRepository(context);

            var result = await repository.DeleteAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result);
        }

    }
}