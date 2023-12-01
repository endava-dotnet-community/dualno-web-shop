using DatabaseEF.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using System.Data.Entity;
using WebShop.DatabaseEF.Entities;

namespace DatabaseEFUnitTests
{
    public class DatabaseUnitTests
    {
        public WebshopContext CreateDbContext()
        {
            var _contextOptions = new DbContextOptionsBuilder<WebshopContext>()
                .UseInMemoryDatabase("WebShopUnitTests")
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            var _context = new WebshopContext(_contextOptions);
            _context.Database.EnsureDeleted();

            return _context;
        }
    }
}
