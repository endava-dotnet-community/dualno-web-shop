using Moq;
using Core.Abstractions.Repositories;
using FluentValidation;
using Models.ViewModels;
using Services;
using Domain;
using Models.Validators;
using Services.Exceptions;

namespace ServicesUnitTests
{
    [TestClass]
    public class ProductServiceUnitTests
    {

        private async Task<Product> GetProduct(long id)
        {
            var product = new Product()
            {
                Id = id,
                Name = "Test1",
                CategoryId = 1,
                Price = 120,
                Description = "Description",
            };
            return product;
        }

        public List<Product> AllProducts { get; set; } = new List<Product>()
        {
            new Product
            {
                Id = 1,
                Name = "Test1",
                CategoryId = 1,
                Description = "Description",
                Price = 100
            },
            new Product
            {
                Id = 2,
                Name = "Test2",
                CategoryId = 2,
                Description = "Description1",
                Price = 200
            },
            new Product
            {
                Id = 3,
                Name = "Test3",
                Description = "Description2",
                Price = 300
            }
        };

        private async Task<List<Product>> GetProducts()
        {
            return AllProducts;
        }

        private void InsertProduct(Product product)
        {
            AllProducts.Add(product);
        }

        private void UpdateProduct(long id, Product product)
        {
            List<Product> novaLista = new List<Product>();
            foreach(var p in AllProducts)
            {
                if(p.Id == id)
                {
                    novaLista.Add(product);
                }
                else
                {
                    novaLista.Add(p);
                }
            }
            AllProducts = novaLista;
        }

        private void DeleteProduct(long id)
        {
            AllProducts.Remove(AllProducts.First(p => p.Id == id));
        }

        private List<Product> SearchByKeyword(string keyword)
        {
            return AllProducts
                .Where(p =>
                    p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    p.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        [TestMethod]
        public void GetByIdAsyncTestMethod()
        {
            var repostitoryMock = new Mock<IProductRepository>(); 
            var categoryRepositoryMock = new Mock<ICategoryRepository>();

            var viewModelValidatorMock = new Mock<IValidator<ProductViewModel>>();


            repostitoryMock
                .Setup(repos => repos.GetByIdAsync(1))
                .Returns(GetProduct(1));

            var service = new ProductsService(repostitoryMock.Object, categoryRepositoryMock.Object, viewModelValidatorMock.Object);

            var result = service.GetByIdAsync(1);

            Assert.IsNotNull(result?.Result);
            Assert.IsInstanceOfType(result, typeof(Task<ProductViewModel>));
            Assert.AreEqual(1, result?.Result.Id);

            var result2 = service.GetByIdAsync(0);

            Assert.IsNotNull(result2.Exception);
        }

        [TestMethod]
        public void GetAllProductsAsyncTestMethod()
        {
            var repostitoryMock = new Mock<IProductRepository>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();

            var viewModelValidatorMock = new Mock<IValidator<ProductViewModel>>();

            repostitoryMock
                .Setup(repos => repos.GetAllProductsAsync())
                .Returns(GetProducts());

            var service = new ProductsService(repostitoryMock.Object, categoryRepositoryMock.Object, viewModelValidatorMock.Object);

            var result = service.GetAllProductsAsync();

            Assert.IsNotNull(result?.Result);
            Assert.AreEqual(3, result.Result.Count);
            Assert.IsInstanceOfType(result, typeof(Task<List<ProductViewModel>>));
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
            var viewModel = new ProductViewModel
            {
                Id = 6,
                Name = "Test6",
                Category = new Category
                {
                    Id = 7,
                    Name = "Test7"
                },
                Description = "Description6",
                Price = 600
            };

            var product = new Product
            {
                Id = 6,
                Name = "Test7",
                Description = "Description7",
                CategoryId = 7,
                Price = 700
            };

            var repostitoryMock = new Mock<IProductRepository>();
            var categoryRepostitoryMock = new Mock<ICategoryRepository>();

            var viewModelValidatorMock = new ProductViewModelValidator();

            repostitoryMock
                .Setup(repos => repos.InsertAsync(It.IsAny<Product>()))
                .Callback(() => InsertProduct(product))
                .Returns(Task.FromResult(true));

            var service = new ProductsService(repostitoryMock.Object, categoryRepostitoryMock.Object, viewModelValidatorMock);

            service.InsertAsync(viewModel);

            Assert.AreEqual(4, AllProducts.Count);
            Assert.AreEqual(6, AllProducts[3].Id);
        }

        [TestMethod]
        public void UpdateAsyncTestMethod()
        {
            var viewModel = new ProductViewModel
            {
                Id = 1,
                Name = "Izmenjeni proizvod 1",
                Category = new Category
                {
                    Id = 1,
                    Name = "Kategorija 1"
                },
                Description = "Izmenjeni opis",
                Price = 100
            };


            var product = new Product
            {
                Id = 1,
                Name = "Izmenjeni proizvod 1",
                Description = "Izmenjeni opis",
                CategoryId = 1,
                Price = 100
            };


            var repostitoryMock = new Mock<IProductRepository>();
            var categoryRepostitoryMock = new Mock<ICategoryRepository>();

            var viewModelValidatorMock = new ProductViewModelValidator();

            repostitoryMock
                .Setup(repos => repos.UpdateAsync(It.IsAny<long>(), It.IsAny<Product>()))
                .Callback(() => UpdateProduct(product.Id, product))
                .Returns(Task.FromResult(true));

            var service = new ProductsService(repostitoryMock.Object, categoryRepostitoryMock.Object, viewModelValidatorMock);

            service.UpdateAsync(viewModel.Id, viewModel);

            Assert.AreEqual(3, AllProducts.Count);
            Assert.AreEqual(viewModel.Price, AllProducts.First(p => p.Id == viewModel.Id).Price);
        }

        [TestMethod]
        public void DeleteAsyncTestMethod()
        {
            const long id = 1;
            var repostitoryMock = new Mock<IProductRepository>();
            var categoryRepostitoryMock = new Mock<ICategoryRepository>();

            var viewModelValidatorMock = new ProductViewModelValidator();

            repostitoryMock
                .Setup(repos => repos.DeleteAsync(It.IsAny<long>()))
                .Callback(() => DeleteProduct(id))
                .Returns(Task.FromResult(true));

            var service = new ProductsService(repostitoryMock.Object, categoryRepostitoryMock.Object, viewModelValidatorMock);

            service.DeleteAsync(id);

            Assert.AreEqual(2, AllProducts.Count);
            Assert.IsNull(AllProducts.FirstOrDefault(p => p.Id == id));
        }

        [TestMethod]
        public void DeleteAsyncNotDeletedExceptionTestMethod()
        {
            const long id = 1;
            var repostitoryMock = new Mock<IProductRepository>();
            var categoryRepostitoryMock = new Mock<ICategoryRepository>();

            var viewModelValidatorMock = new ProductViewModelValidator();

            repostitoryMock
                .Setup(repos => repos.DeleteAsync(It.IsAny<long>()))
                .Returns(Task.FromResult(false));

            var service = new ProductsService(repostitoryMock.Object, categoryRepostitoryMock.Object, viewModelValidatorMock);

            var result = service.DeleteAsync(id);

            Assert.IsNotNull(result.Exception);
            Assert.AreEqual(result.Exception?.InnerException?.GetType(), typeof(ResourceNotFoundException));
        }

        [TestMethod]
        public void SearchByKeyWordAsyncTestMethod()
        {
            var repostitoryMock = new Mock<IProductRepository>();
            var categoryRepostitoryMock = new Mock<ICategoryRepository>();

            var viewModelValidatorMock = new ProductViewModelValidator();

            repostitoryMock
                .Setup(repos => repos.SearchByKeyWordAsync("Test1"))
                .Callback(() => SearchByKeyword("Test1"))
                .Returns(Task.FromResult(SearchByKeyword("Test1")));

            var service = new ProductsService(repostitoryMock.Object, categoryRepostitoryMock.Object, viewModelValidatorMock);

            var result = service.SearchByKeyWordAsync("Test1");

            Assert.IsNotNull(result);
            Assert.AreEqual(result?.Result.Count, 1);
        }
    }
}