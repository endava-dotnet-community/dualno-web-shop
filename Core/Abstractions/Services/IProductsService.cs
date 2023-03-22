using Domain;
using Models.ViewModels;

namespace Core.Abstractions.Services
{
    public interface IProductsService
    {
        ProductViewModel? GetById(long productId);
        List<ProductViewModel?> GetAllProducts();
        void Insert(ProductViewModel product);
        bool Update(long productId, ProductViewModel product);
        bool Delete(long productId);
        List<ProductViewModel?> SearchByKeyWord(string keyoword);
    }
}