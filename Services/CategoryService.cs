using Core.Abstractions.Repositories;
using Core.Abstractions.Services;
using Domain;
using FluentValidation;
using Models.Validators;
using Models.ViewModels;
using Services.Exceptions;

namespace Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IValidator<CategoryViewModel> _viewModelValidator;

        public CategoryService(
            ICategoryRepository repository,
            IValidator<CategoryViewModel> viewModelValidator)
        {
            _repository = repository;
            _viewModelValidator = viewModelValidator;
        }

        public async Task<CategoryViewModel> GetByIdAsync(long categoryId)
        {
            var foundCategory = MapToViewModel(await _repository.GetByIdAsync(categoryId));

            if (foundCategory == null)
                throw new ResourceNotFoundException($"Category with id: {categoryId} was not found.");

            return foundCategory;
        }

        public async Task<List<CategoryViewModel>> GetAllCategoriesAsync()
        {
            return (await _repository
                .GetAllCategoriesAsync())
                .Select<Category, CategoryViewModel>(p => MapToViewModel(p))
                .Where(p => p != null)
                .ToList();
        }

        public async Task InsertAsync(CategoryViewModel categoryViewModel)
        {
            _viewModelValidator.ValidateAndThrow(categoryViewModel);

            Category Category = MapFromViewModel(categoryViewModel);

            if (Category == null)
                throw new ArgumentNullException(nameof(categoryViewModel));

            await _repository.InsertAsync(Category);
        }

        public async Task<bool> UpdateAsync(long categoryId, CategoryViewModel categoryViewModel)
        {
            Category Category = MapFromViewModel(categoryViewModel);

            if (Category == null)
                throw new ArgumentNullException(nameof(categoryViewModel));

            return await _repository.UpdateAsync(categoryId, Category);
        }

        public async Task<bool> DeleteAsync(long categoryId)
        {
            bool isDeleted = await _repository.DeleteAsync(categoryId);
            if (!isDeleted)
            {
                throw new ResourceNotFoundException($"Category with id: {categoryId} was not found!");
            }
            return true;
        }

        public async Task<List<CategoryViewModel>> SearchByKeyWordAsync(string keyword)
        {
            return (await _repository
                .GetAllCategoriesAsync())
                .Where(c => c.Name.Contains(keyword, StringComparison.InvariantCultureIgnoreCase))
                .Select<Category, CategoryViewModel>(p => MapToViewModel(p))
                .ToList();
        }

        private CategoryViewModel MapToViewModel(Category p)
        {
            if (p == null)
                return null;

            return new CategoryViewModel
            {
                Id = p.Id,
                Name = p.Name,
            };
        }

        private Category MapFromViewModel(CategoryViewModel p)
        {
            if (p == null)
                return null;

            return new Category
            {
                Id = p.Id,
                Name = p.Name,
            };
        }

    }
}