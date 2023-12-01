using DatabaseEF.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using WebShop.DatabaseEF.Entities;

namespace DatabaseEFUnitTests
{
    [TestClass]
    public class CategoryRepositoryUnitTests : DatabaseUnitTests
    {
        [TestMethod]
        public async Task GetByIdAsyncTestMethod()
        {
            using var context = CreateDbContext();
            var repository = new CategoryRepository(context);

            await repository.InsertAsync(
                new Domain.Category
                {
                    Id = 1,
                    Name = "Test1"
                });

            var result = await repository.GetByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }
    }
}