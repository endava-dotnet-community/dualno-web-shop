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

        public async Task<ProductViewModel> GetByIdAsync(long productId)
        {
            var foundProduct = MapToViewModel(await _repository.GetByIdAsync(productId));

            if (foundProduct == null)
                throw new ResourceNotFoundException($"Product with id: {productId} was not found.");

            return foundProduct;
        }

        public async Task<List<ProductViewModel>> GetAllProductsAsync()
        {
            return (await _repository
                .GetAllProductsAsync())
                .Select<Product, ProductViewModel>(p => MapToViewModel(p))
                .Where(p => p != null)
                .ToList();
        }

        public async Task InsertAsync(ProductViewModel productViewModel)
        {
            _viewModelValidator.ValidateAndThrow(productViewModel);

            Product product = MapFromViewModel(productViewModel);

            if (product == null)
                throw new ArgumentNullException(nameof(productViewModel));

            await _repository.InsertAsync(product);
        }

        public async Task<bool> UpdateAsync(long productId, ProductViewModel productViewModel)
        {
            Product product = MapFromViewModel(productViewModel);

            if (product == null)
                throw new ArgumentNullException(nameof(productViewModel));

            return  await _repository.UpdateAsync(productId, product);
        }

        public async Task<bool> DeleteAsync(long productId)
        {
            bool isDeleted = await _repository.DeleteAsync(productId);
            if (!isDeleted)
            {
                throw new ResourceNotFoundException($"Product with id: {productId} was not found!");
            }
            return true;
        }

        public async Task<List<ProductViewModel>> SearchByKeyWordAsync(string keyword)
        {
            return (await _repository
                .SearchByKeyWordAsync(keyword))
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
                Category = _categoryRepository.GetByIdAsync(p.CategoryId).Result,
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