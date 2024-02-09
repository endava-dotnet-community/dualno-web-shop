using Moq;
using Core.Abstractions.Repositories;
using FluentValidation;
using Models.ViewModels;
using Services;
using Domain;
using Models.Validators;
using Core.Abstractions.Services;

namespace ServicesUnitTests
{
    [TestClass]
    public class ShoppingCartServiceUnitTests
    {

        private async Task<ShoppingCart> GetShoppingCart()
        {
            var shoppingcart = new ShoppingCart()
            {
                Id = 1,
                AccessedAt = new DateTime(2024, 1, 1, 0, 0, 0),
                SessionId = "session1",
                Items = new List<ShoppingCartItem>()
                {
                    new (){
                        Id = 1,
                        CartId = 1,
                        ProductId = 1,
                        Quantity = 100
                    }
                }
            };
            return shoppingcart;
        }

        [TestMethod]
        public async Task GetBySessionIdAsyncTestMethod()
        {
            var repostitoryMock = new Mock<IShoppingCartRepository>();

            var viewModelValidatorMock = new Mock<IValidator<ShoppingCartViewModel>>();
            var sessionId = "session1";

            repostitoryMock
                .Setup(repos => repos.GetBySessionIdAsync(sessionId))
                .Returns(GetShoppingCart());

            IShoppingCartService service = new ShoppingCartService(repostitoryMock.Object, viewModelValidatorMock.Object);


            var result = await service.GetBySessionIdAsync(sessionId);

            Assert.IsNotNull(result?.SessionId);
            Assert.AreEqual(sessionId, result.SessionId);
            Assert.IsInstanceOfType(result, typeof(ShoppingCartViewModel));
        }

    }
}