using Core.Abstractions.Services;
using Models.ViewModels;
using Moq;
using WebShop.Controllers;

namespace WebShopUnitTests
{
    [TestClass]
    public class ProductsControllerUnitTest
    {


        private async Task<List<ProductViewModel>> GetProducts()
        {
            List<ProductViewModel> productsViewModels = new List<ProductViewModel>
            {
                new ProductViewModel
                {
                    Id = 1,
                    Category = new Domain.Category
                    {
                        Id = 1,
                        Name = "Prehrambeni proizvodi"
                    },
                    Description = "Kafa",
                    Name = "Grand kafa",
                    Price = 200
                }
             };
            return productsViewModels;
        }
            

        [TestMethod]
        public void GetAllProductsTest()
        {

            var productsViewModels = GetProducts().Result;

            var productServiceMock = new Mock<IProductsService>();
            productServiceMock
                .Setup(service => service.GetAllProductsAsync())
                .Returns(GetProducts());

            var userServiceMock = new Mock<IUsersService>();


            var controller = new ProductsController(productServiceMock.Object, userServiceMock.Object);

            //Act

            var result = controller.GetAllProducts();


            Assert.IsNotNull(result);
            Assert.AreEqual(result.Result.Count, productsViewModels.Count);
            Assert.AreEqual(result.Result[0].Id, productsViewModels[0].Id);

        }


        [TestMethod]
        public void GetAllProductsServiceExceptionTest()
        {

            var productServiceMock = new Mock<IProductsService>();
            productServiceMock
                .Setup(service => service.GetAllProductsAsync())
                .Throws(new Exception());

            var userServiceMock = new Mock<IUsersService>();


            var controller = new ProductsController(productServiceMock.Object, userServiceMock.Object);

            //Act

            var result = controller.GetAllProducts();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Task));
            Assert.IsNull(result.Result);

        }

        [TestMethod]
        public void InsertTestMethod()
        {

            var productServiceMock = new Mock<IProductsService>();
            productServiceMock
                .Setup(service => service.GetAllProductsAsync())
                .Throws(new Exception());

            var userServiceMock = new Mock<IUsersService>();


            var controller = new ProductsController(productServiceMock.Object, userServiceMock.Object);

            //Act

            var result = controller.GetAllProducts();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Task));
            Assert.IsNull(result.Result);

        }
    }
}