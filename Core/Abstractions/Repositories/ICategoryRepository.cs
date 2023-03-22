using Domain;

namespace Core.Abstractions.Repositories
{
    public interface ICategoryRepository
    {
        Category? GetById(long categoryId);
        List<Category> GetAllCategories();
        bool Insert(Category category);
        bool Update(long categoryId, Category category);
        bool Delete(long categoryId);
    }
}