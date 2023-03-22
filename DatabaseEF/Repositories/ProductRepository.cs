using Core.Abstractions.Repositories;
using Domain;
using Models.ViewModels;
using System;
using System.Collections.Generic;
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

        public bool Delete(long productId)
        {
            ProductEntity? entity = _context.Products.Find(productId);

            if (entity == null)
                return false;

            _context.Products.Remove(entity);
            _context.SaveChanges();

            return true;
        }

        public List<Product> GetAllProducts()
        {
            return _context
                .Products
                .Select(e => MapFromEntity(e))
                .ToList();
        }

        public Product? GetById(long productId)
        {
            ProductEntity? entity = _context.Products.Find(productId);
            return MapFromEntity(entity);
        }

        public bool Insert(Product? product)
        {
            if (product == null)
                return false;

            ProductEntity entity = MapToEntity(product);
            _context.Products.Add(entity);
            _context.SaveChanges();
            
            return true;
        }

        public List<Product> SearchByKeyWord(string keyword)
        {
            throw new NotImplementedException();
        }

        public bool Update(long productId, Product product)
        {
            ProductEntity? entity = _context.Products.Find(productId);

            if (entity == null)
                return false;

            entity.Name = product.Name;
            entity.Description = product.Description;
            entity.Price = product.Price;
            entity.CategoryId = product.CategoryId;

            _context.SaveChanges();

            return true;
        }

        private ProductEntity MapToEntity(Product? p)
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

        private static Product MapFromEntity(ProductEntity? p)
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
