using Core.Abstractions.Repositories;
using Core.Abstractions.Services;
using Core.Domain;

namespace Services
{
    public class ProductsService : IProductsService
    {
        private readonly IProductRepository _repository;

        public ProductsService(IProductRepository productRepository)
        {
            _repository = productRepository;
        }

        public List<Proizvod> GetAllProducts()
        {
            return _repository.GetAllProducts();
        }

        public void InsertProduct(Proizvod product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            _repository.Insert(product);
        }
    }
}