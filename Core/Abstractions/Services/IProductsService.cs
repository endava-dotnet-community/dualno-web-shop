using Domain;
using Models.ViewModels;

namespace Core.Abstractions.Services
{
    public interface IProductsService
    {
        Task<ProductViewModel?> GetByIdAsync(long productId);
        Task<List<ProductViewModel?>> GetAllProductsAsync();
        Task InsertAsync(ProductViewModel product);
        Task<bool> UpdateAsync(long productId, ProductViewModel product);
        Task<bool> DeleteAsync(long productId);
        Task<List<ProductViewModel?>> SearchByKeyWordAsync(string keyoword);
    }
}