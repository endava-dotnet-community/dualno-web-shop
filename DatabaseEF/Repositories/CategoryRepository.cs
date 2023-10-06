using Core.Abstractions.Repositories;
using Domain;
using System.Data.Entity;
using WebShop.DatabaseEF.Entities;

namespace DatabaseEF.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        readonly WebshopContext _context;
        public CategoryRepository(WebshopContext dbContext)
        {
            _context = dbContext;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<List<Category>> GetAllCategoriesAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return _context
                .Categories
                .AsNoTracking()
                .Select(c => MapFromEntity(c))
                .ToList();
        }

        public async Task<Category> GetByIdAsync(long categoryId)
        {
            return MapFromEntity(await _context.Categories.FindAsync(categoryId));
        }

        public async Task<bool> InsertAsync(Category category)
        {
            if(category == null) 
                return false;

            CategoryEntity entity = MapToEntity(category);

            if(entity== null)
                return false;

            await _context.Categories.AddAsync(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(long categoryId, Category category)
        {
            CategoryEntity entity = await _context.Categories.FindAsync(categoryId);
            
            if(entity== null) 
                return false;
            
            entity.Name= category.Name;
            
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> DeleteAsync(long categoryId)
        {
            CategoryEntity entity = await _context.Categories.FindAsync(categoryId);

            if (entity == null)
                return false;

            _context.Categories.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        private static Category MapFromEntity(CategoryEntity entity)
        {
            if(entity == null) 
                return null;

            return new Category
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        private static CategoryEntity MapToEntity(Category category)
        {
            if(category == null) 
                return null;

            return new CategoryEntity
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
}
