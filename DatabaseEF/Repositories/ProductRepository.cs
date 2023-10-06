using Core.Abstractions.Repositories;
using Domain;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.DatabaseEF.Entities;

namespace DatabaseEF.Repositories
{
    public class ProductRepository : IProductRepository
    {
        readonly WebshopContext _context;
        public ProductRepository(WebshopContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<bool> DeleteAsync(long productId)
        {
            ProductEntity entity = await _context.Products.FindAsync(productId);

            if (entity == null)
                return false;

            _context.Products.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return _context
                .Products
                .AsNoTracking()
                .Select(e => MapFromEntity(e))
                .ToList();
        }

        public async Task<Product> GetByIdAsync(long productId)
        {
            ProductEntity entity = (await _context.Products.FindAsync(productId));
            return MapFromEntity(entity);
        }

        public async Task<bool> InsertAsync(Product product)
        {
            if (product == null)
                return false;

            ProductEntity entity = MapToEntity(product);
            await _context.Products.AddAsync(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Product>> SearchByKeyWordAsync(string keyword)
        {
            return _context
                .Products
                .AsNoTracking()
                .ToList()
                .Where(p =>
                    p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    p.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .Select(p => MapFromEntity(p))
                .ToList();
        }

        public async Task<bool> UpdateAsync(long productId, Product product)
        {
            ProductEntity entity = await _context.Products.FindAsync(productId);

            if (entity == null)
                return false;

            entity.Name = product.Name;
            entity.Description = product.Description;
            entity.Price = product.Price;
            entity.CategoryId = product.CategoryId;

            await _context.SaveChangesAsync();

            return true;
        }

        private static ProductEntity MapToEntity(Product p)
        {
            if (p == null)
                return null;

            return new ProductEntity
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CategoryId = p.CategoryId,
                Description = p.Description,
            };
        }

        private static Product MapFromEntity(ProductEntity p)
        {
            if (p == null)
                return null;

            return new Product
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CategoryId = p.CategoryId,
                Description = p.Description,
            };
        }

        private static Category MapFromCategoryEntity(CategoryEntity categoryEntity)
        {
            return new Category
            {
                Id = categoryEntity.Id,
                Name = categoryEntity.Name,
            };
        }

    }
}
