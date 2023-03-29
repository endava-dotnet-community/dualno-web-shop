using Domain;

namespace Core.Abstractions.Repositories
{
    public interface IProductRepository
    {
        Product GetById(long productId);
        List<Product> GetAllProducts();
        bool Insert(Product product);
        bool Update(long productId, Product product);
        bool Delete(long productId);
        List<Product> SearchByKeyWord(string keyword);
    }
}