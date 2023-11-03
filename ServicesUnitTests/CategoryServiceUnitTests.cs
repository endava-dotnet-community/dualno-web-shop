using Moq;
using Core.Abstractions.Repositories;
using FluentValidation;
using Models.ViewModels;
using Services;
using Domain;
using Models.Validators;

namespace ServicesUnitTests
{
    [TestClass]
    public class CategoryServiceUnitTests
    {

        private async Task<Category> GetCategory(long id)
        {
            var category = new Category()
            {
                Id = id,
                Name = "Test1"
            };
            return category;
        }

        public List<Category> AllCategories { get; set; } = new List<Category>()
        {
            new Category
            {
                Id = 1,
                Name = "Test1"
            },
            new Category
            {
                Id = 2,
                Name = "Test2"
            },
            new Category
            {
                Id = 3,
                Name = "Test3"
            }
        };

        private async Task<List<Category>> GetCategories()
        {
            return AllCategories;
        }

        private void InsertCategory(Category category)
        {
            AllCategories.Add(category);
        }

        [TestMethod]
        public void GetByIdAsyncTestMethod()
        {
            var repostitoryMock = new Mock<ICategoryRepository>();

            var viewModelValidatorMock = new Mock<IValidator<CategoryViewModel>>();

            repostitoryMock
                .Setup(repos => repos.GetByIdAsync(1))
                .Returns(GetCategory(1));

            var service = new CategoryService(repostitoryMock.Object, viewModelValidatorMock.Object);

            var result = service.GetByIdAsync(1);

            Assert.IsNotNull(result?.Result);
            Assert.IsInstanceOfType(result, typeof(Task<CategoryViewModel>));
            Assert.AreEqual(1, result.Id);

            var result2 = service.GetByIdAsync(0);

            Assert.IsNotNull(result2.Exception);
        }

        [TestMethod]
        public void GetAllCategoriesAsyncTestMethod()
        {
            var repostitoryMock = new Mock<ICategoryRepository>();

            var viewModelValidatorMock = new Mock<IValidator<CategoryViewModel>>();

            repostitoryMock
                .Setup(repos => repos.GetAllCategoriesAsync())
                .Returns(GetCategories());

            var service = new CategoryService(repostitoryMock.Object, viewModelValidatorMock.Object);

            var result = service.GetAllCategoriesAsync();

            Assert.IsNotNull(result?.Result);
            Assert.AreEqual(3, result.Result.Count);
            Assert.IsInstanceOfType(result, typeof(Task<List<CategoryViewModel>>));
        }

        [TestMethod]
        public void GetAllCategoriesAsyncRepositoryExceptionTestMethod()
        {
            var repostitoryMock = new Mock<ICategoryRepository>();

            var viewModelValidatorMock = new Mock<IValidator<CategoryViewModel>>();

            repostitoryMock
                .Setup(repos => repos.GetAllCategoriesAsync())
                .Throws(new OverflowException());

            var service = new CategoryService(repostitoryMock.Object, viewModelValidatorMock.Object);

            var result = service.GetAllCategoriesAsync();

            Assert.IsNotNull(result.Exception);
        }

        [TestMethod]
        public void InsertAsyncTestMethod()
        {
            var viewModel = new CategoryViewModel
            {
                Id = 6,
                Name = "Test6"
            };

            var category = new Category
            {
                Id = 6,
                Name = "Test6"
            };

            var repostitoryMock = new Mock<ICategoryRepository>();

            var viewModelValidatorMock = new CategoryViewModelValidator();

            Action insertCategoryAction = () => InsertCategory(category);

            repostitoryMock
                .Setup(repos => repos.InsertAsync(It.IsAny<Category>()))
                .Callback(() => InsertCategory(category))
                .Returns(Task.FromResult(true));

            var service = new CategoryService(repostitoryMock.Object, viewModelValidatorMock);

            service.InsertAsync(viewModel);

            Assert.AreEqual(4, AllCategories.Count);
            Assert.AreEqual(6, AllCategories[3].Id);
        }
    }
}