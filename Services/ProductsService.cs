using Core.Abstractions.Repositories;
using Core.Abstractions.Services;
using Domain;
using FluentValidation;
using Models.Validators;
using Models.ViewModels;
using Services.Exceptions;

namespace Services
{
    public class ProductsService : IProductsService
    {
        private readonly IProductRepository _repository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IValidator<ProductViewModel> _viewModelValidator;

        public ProductsService(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IValidator<ProductViewModel> viewModelValidator)
        {
            _repository = productRepository;
            _categoryRepository = categoryRepository;
            _viewModelValidator = viewModelValidator;
        }

        public ProductViewModel GetById(long productId)
        {
            var foundProduct = MapToViewModel(_repository.GetById(productId));

            if (foundProduct == null)
                throw new ResourceNotFoundException($"Product with id: {productId} was not found.");

            return foundProduct;
        }

        public List<ProductViewModel> GetAllProducts()
        {
            return _repository
                .GetAllProducts()
                .Select<Product, ProductViewModel>(p => MapToViewModel(p))
                .Where(p => p != null)
                .ToList();
        }

        public void Insert(ProductViewModel productViewModel)
        {
            _viewModelValidator.ValidateAndThrow(productViewModel);

            Product product = MapFromViewModel(productViewModel);

            if (product == null)
                throw new ArgumentNullException(nameof(productViewModel));

            _repository.Insert(product);
        }

        public bool Update(long productId, ProductViewModel productViewModel)
        {
            Product product = MapFromViewModel(productViewModel);

            if (product == null)
                throw new ArgumentNullException(nameof(productViewModel));

            return _repository.Update(productId, product);
        }

        public bool Delete(long productId)
        {
            bool isDeleted = _repository.Delete(productId);
            if (!isDeleted)
            {
                throw new ResourceNotFoundException($"Product with id: {productId} was not found!");
            }
            return true;
        }

        public List<ProductViewModel> SearchByKeyWord(string keyword)
        {
            return _repository
                .SearchByKeyWord(keyword)
                .Select<Product, ProductViewModel>(p => MapToViewModel(p))
                .Where(p => p != null)
                .ToList();
        }

        private ProductViewModel MapToViewModel(Product p)
        {
            if (p == null)
                return null;

            return new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Category = _categoryRepository.GetById(p.CategoryId),
                Description = p.Description,
            };
        }

        private Product MapFromViewModel(ProductViewModel p)
        {
            if (p == null)
                return null;

            return new Product
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CategoryId = p.Category.Id,
                Description = p.Description,
            };
        }

    }
}