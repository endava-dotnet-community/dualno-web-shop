using Domain;
using Models.ViewModels;

namespace Core.Abstractions.Services
{
    public interface ICategoryService
    {
        Task<CategoryViewModel> GetByIdAsync(long productId);
        Task<List<CategoryViewModel>> GetAllCategoriesAsync();
        Task InsertAsync(CategoryViewModel product);
        Task<bool> UpdateAsync(long productId, CategoryViewModel product);
        Task<bool> DeleteAsync(long productId);
        Task<List<CategoryViewModel>> SearchByKeyWordAsync(string keyoword);
    }
}