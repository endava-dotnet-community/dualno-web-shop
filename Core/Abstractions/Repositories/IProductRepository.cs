using Domain;

namespace Core.Abstractions.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(long productId);
        Task<List<Product>> GetAllProductsAsync();
        Task<bool> InsertAsync(Product product);
        Task<bool> UpdateAsync(long productId, Product product);
        Task<bool> DeleteAsync(long productId);
        Task<List<Product>> SearchByKeyWordAsync(string keyword);
    }
}