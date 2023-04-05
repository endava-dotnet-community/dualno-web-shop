using Domain;
using Models.ViewModels;

namespace Core.Abstractions.Services
{
    public interface ICategoryService
    {
        CategoryViewModel GetById(long productId);
        List<CategoryViewModel> GetAllCategories();
        void Insert(CategoryViewModel product);
        bool Update(long productId, CategoryViewModel product);
        bool Delete(long productId);
        List<CategoryViewModel> SearchByKeyWord(string keyoword);
    }
}