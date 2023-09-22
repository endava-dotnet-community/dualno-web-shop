using Domain;

namespace Core.Abstractions.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> GetByIdAsync(long categoryId);
        Task<List<Category>> GetAllCategoriesAsync();
        Task<bool> InsertAsync(Category category);
        Task<bool> UpdateAsync(long categoryId, Category category);
        Task<bool> DeleteAsync(long categoryId);
    }
}