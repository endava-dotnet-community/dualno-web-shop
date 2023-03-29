using Core.Abstractions.Repositories;
using Domain;
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

        public bool Delete(long categoryId)
        {
            CategoryEntity entity = _context.Categories.Find(categoryId);

            if (entity == null)
                return false;

            _context.Categories.Remove(entity);
            _context.SaveChanges();

            return true;
        }

        public List<Category> GetAllCategories()
        {
            return _context
                .Categories
                .Select(c => MapFromEntity(c))
                .ToList();
        }

        public Category GetById(long categoryId)
        {
            return MapFromEntity(_context.Categories.Find(categoryId));
        }

        public bool Insert(Category category)
        {
            if(category == null) 
                return false;

            CategoryEntity entity = MapToEntity(category);

            if(entity== null)
                return false;

            _context.Categories.Add(entity);
            _context.SaveChanges();

            return true;
        }

        public bool Update(long categoryId, Category category)
        {
            CategoryEntity entity = _context.Categories.Find(categoryId);
            
            if(entity== null) 
                return false;
            
            entity.Name= category.Name;
            
            _context.SaveChanges();
            
            return true;
        }

        private Category MapFromEntity(CategoryEntity entity)
        {
            if(entity == null) 
                return null;

            return new Category
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        private CategoryEntity MapToEntity(Category category)
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
