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

        public List<ShoppingCart> AllShoppingCarts { get; set; } = new List<ShoppingCart>()
        {
            new ShoppingCart
            {
                Id = 2,
                AccessedAt = new DateTime(2024, 1, 2, 0, 0, 0),
                SessionId = "session2",
                Items = new List<ShoppingCartItem>()
                {
                    new (){
                        Id = 2,
                        CartId = 2,
                        ProductId = 2,
                        Quantity = 100
                    }
                }
            },
            new ShoppingCart
            {
                Id = 3,
                AccessedAt = new DateTime(2024, 1, 3, 0, 0, 0),
                SessionId = "session3",
                Items = new List<ShoppingCartItem>()
                {
                    new (){
                        Id = 3,
                        CartId = 3,
                        ProductId = 3,
                        Quantity = 100
                    }
                }
            },
            new ShoppingCart
            {
                Id = 4,
                AccessedAt = new DateTime(2024, 1, 4, 0, 0, 0),
                SessionId = "session4",
                Items = new List<ShoppingCartItem>()
                {
                    new (){
                        Id = 4,
                        CartId = 4,
                        ProductId = 4,
                        Quantity = 100
                    }
                }
            }
        };
        private async Task<List<ShoppingCart>> GetShoppingCarts()//david
        {
            return AllShoppingCarts;
        }

        private void InsertShoppingCart(ShoppingCart shoppingCart)//fica
        {
            AllShoppingCarts.Add(shoppingCart);
        }

        [TestMethod]
        public async Task GetBySessionIdAsyncTestMethod()
        {
            var repositoryMock = new Mock<IShoppingCartRepository>();

            var viewModelValidatorMock = new Mock<IValidator<ShoppingCartViewModel>>();
            var itemViewModelValidatorMock = new Mock<IValidator<ShoppingCartItemViewModel>>();
            var sessionId = "session1";

            repositoryMock
                .Setup(repos => repos.GetBySessionIdAsync(sessionId))
                .Returns(GetShoppingCart());

            IShoppingCartService service = new ShoppingCartService(repositoryMock.Object, viewModelValidatorMock.Object, itemViewModelValidatorMock.Object);


            var result = await service.GetBySessionIdAsync(sessionId);

            Assert.IsNotNull(result?.SessionId);
            Assert.AreEqual(sessionId, result.SessionId);
            Assert.IsInstanceOfType(result, typeof(ShoppingCartViewModel));
        }

        [TestMethod]
        public async Task DeleteShoppingCartAsyncTestMethod()//vincic
        {
            var repositoryMock = new Mock<IShoppingCartRepository>();
            var viewModelValidatorMock = new Mock<IValidator<ShoppingCartViewModel>>();
            var itemViewModelValidatorMock = new Mock<IValidator<ShoppingCartItemViewModel>>();
            var Id = 1;
            repositoryMock
                .Setup(repos => repos.DeleteShoppingCartAsync(Id))
                .Returns(Task.FromResult(true));
            IShoppingCartService service = new ShoppingCartService(repositoryMock.Object, viewModelValidatorMock.Object, itemViewModelValidatorMock.Object);
            var result = await service.DeleteShoppingCartAsync(Id);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task DeleteShoppingCartItemAsyncTestMethod()//mihajlo
        {
            var repositoryMock = new Mock<IShoppingCartRepository>();
            var viewModelValidatorMock = new Mock<IValidator<ShoppingCartViewModel>>();
            var itemViewModelValidatorMock = new Mock<IValidator<ShoppingCartItemViewModel>>();
            int Id = 1;

            repositoryMock.Setup(repos => repos.DeleteShoppingCartItemAsync(Id)).Returns(Task.FromResult(true));

            IShoppingCartService service = new ShoppingCartService(repositoryMock.Object, viewModelValidatorMock.Object, itemViewModelValidatorMock.Object);

            var result = await service.DeleteShoppingCartItemAsync(Id);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task GetAllShoppingCartsAsyncTestMethod()//david
        {
            var repositoryMock = new Mock<IShoppingCartRepository>();
            var viewModelValidatorMock = new Mock<IValidator<ShoppingCartViewModel>>();
            var itemViewModelValidatorMock = new Mock<IValidator<ShoppingCartItemViewModel>>();
            repositoryMock
                .Setup(repos => repos.GetAllShoppingCartsAsync())
                .Returns(GetShoppingCarts());
            IShoppingCartService service = new ShoppingCartService(repositoryMock.Object, viewModelValidatorMock.Object, itemViewModelValidatorMock.Object);
            var result = service.GetAllShoppingCartsAsync();
            Assert.IsNotNull(result?.Result);
            Assert.AreEqual(3, result.Result.Count);
        }

        [TestMethod]
        public async Task InsertShoppingCartAsyncTestMethod() // filip
        {
            var viewModel = new ShoppingCartViewModel
            {
                Id = 3,
                AccessedAt = new DateTime(2005, 5, 11),
                Items = new List<ShoppingCartItemViewModel>()
                {
                    new ShoppingCartItemViewModel(){
                        Id = 1,
                        CartId = 1,
                        ProductId = 1,
                        Quantity = 1
                    },
                    new ShoppingCartItemViewModel(){
                        Id = 2,
                        CartId = 2,
                        ProductId = 2,
                        Quantity = 2
                    }
                },
                SessionId = "session3"
            };
            var shoppingCart = new ShoppingCart
            {
                Id = 4,
                AccessedAt = new DateTime(2007, 5, 20),
                Items = new List<ShoppingCartItem>()
                {
                    new ShoppingCartItem(){
                        Id = 3,
                        CartId = 3,
                        ProductId = 3,
                        Quantity = 3
                    },
                    new ShoppingCartItem(){
                        Id = 4,
                        CartId = 4,
                        ProductId = 4,
                        Quantity = 4
                    }
                },
                SessionId = "session4"
            };
            var repositoryMock = new Mock<IShoppingCartRepository>();
            var viewModelValidatorMock = new Mock<IValidator<ShoppingCartViewModel>>();
            var itemViewModelValidatorMock = new Mock<IValidator<ShoppingCartItemViewModel>>();
            repositoryMock
                .Setup(repos => repos.InsertShoppingCartAsync(shoppingCart))
                .Callback(() => InsertShoppingCart(shoppingCart))
                .Returns(Task.FromResult(true));
            IShoppingCartService service = new ShoppingCartService(repositoryMock.Object, viewModelValidatorMock.Object, itemViewModelValidatorMock.Object);
            await service.InsertShoppingCartAsync(viewModel);
            Assert.AreEqual(3, AllShoppingCarts.Count);
            Assert.AreEqual(4, AllShoppingCarts[2].Id);
        }

        [TestMethod]
        public async Task InsertShoppingCartItemAsyncTestMethod() // lazar
        {
            var repositoryMock = new Mock<IShoppingCartRepository>();
            var viewModelValidatorMock = new Mock<IValidator<ShoppingCartViewModel>>();
            var itemViewModelValidatorMock = new Mock<IValidator<ShoppingCartItemViewModel>>();
            var shoppingCartItemViewModel = new ShoppingCartItemViewModel()
            {
                Id = 1,
                CartId = 1,
                ProductId = 1,
                Quantity = 10
            };
            
            repositoryMock
                .Setup(repos => repos.InsertShoppingCartItemAsync(It.IsAny<ShoppingCartItem>()))
                .Returns(Task.FromResult(true));
            IShoppingCartService service = new ShoppingCartService(repositoryMock.Object, viewModelValidatorMock.Object, itemViewModelValidatorMock.Object);
            var result = await service.InsertShoppingCartItemAsync(shoppingCartItemViewModel);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task UpdateAccessedAtAsyncTestMethod() // luka
        {
            var repositoryMock = new Mock<IShoppingCartRepository>();
            var viewModelValidatorMock = new Mock<IValidator<ShoppingCartViewModel>>();
            var itemViewModelValidatorMock = new Mock<IValidator<ShoppingCartItemViewModel>>();
            var Dt = DateTime.Now;
            repositoryMock
                .Setup(repos => repos.UpdateAccessedAtAsync(1, Dt))
                .Returns(Task.FromResult(true));
            IShoppingCartService service = new ShoppingCartService(repositoryMock.Object, viewModelValidatorMock.Object, itemViewModelValidatorMock.Object);
            var result = await service.UpdateAccessedAtAsync(1, Dt);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task UpdateQuantityAsyncTestMethod() // ilija
        {
            var repositoryMock = new Mock<IShoppingCartRepository>();
            var viewModelValidatorMock = new Mock<IValidator<ShoppingCartViewModel>>();
            var itemViewModelValidatorMock = new Mock<IValidator<ShoppingCartItemViewModel>>();
            var sessionId = Guid.NewGuid().ToString();
            repositoryMock
                .Setup(repos => repos.UpdateQuantityAsync(1, 1))
                .Returns(Task.FromResult(true));
            IShoppingCartService service = new ShoppingCartService(repositoryMock.Object, viewModelValidatorMock.Object, itemViewModelValidatorMock.Object);
            var result = await service.UpdateQuantityAsync(1, 1);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public async Task UpdateNegativeQuantityAsyncTestMethod() // ja
        {
            var repositoryMock = new Mock<IShoppingCartRepository>();
            var viewModelValidatorMock = new Mock<IValidator<ShoppingCartViewModel>>();
            var itemViewModelValidatorMock = new Mock<IValidator<ShoppingCartItemViewModel>>();
            repositoryMock
              .Setup(repos => repos.UpdateQuantityAsync(1, -1))
              .Returns(Task.FromResult(false));
            IShoppingCartService service = new ShoppingCartService(repositoryMock.Object, viewModelValidatorMock.Object, itemViewModelValidatorMock.Object);
            var result = await service.UpdateQuantityAsync(1, -1);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task InsertInvalidShoppingCartItemAsyncTestMethod() // stojkovic
        {
            var repositoryMock = new Mock<IShoppingCartRepository>();
            var viewModelValidatorMock = new Mock<IValidator<ShoppingCartViewModel>>();
            var itemViewModelValidatorMock = new Mock<IValidator<ShoppingCartItemViewModel>>();
            repositoryMock
                .Setup(repos => repos.InsertShoppingCartItemAsync(It.IsAny<ShoppingCartItem>()))
                .Returns(Task.FromResult(false));
            IShoppingCartService service = new ShoppingCartService(repositoryMock.Object, viewModelValidatorMock.Object, itemViewModelValidatorMock.Object);
            var shoppingCartItemViewModel = new ShoppingCartItemViewModel
            {
                Id = 2,
                CartId = 3,
                ProductId = 1,
                Quantity = -1
            };
            var result = await service.InsertShoppingCartItemAsync(shoppingCartItemViewModel);
            Assert.AreEqual(result, false);
        }

        [TestMethod]
        public async Task DeleteInvalidShoppingCartItemAsyncTestMethod() // lara
        {
            var repositoryMock = new Mock<IShoppingCartRepository>();
            var viewModelValidatorMock = new Mock<IValidator<ShoppingCartViewModel>>();
            var itemViewModelValidatorMock = new Mock<IValidator<ShoppingCartItemViewModel>>();
            repositoryMock
                .Setup(repos => repos.DeleteShoppingCartItemAsync(-1))
                .Returns(Task.FromResult(false));
            IShoppingCartService service = new ShoppingCartService(repositoryMock.Object, viewModelValidatorMock.Object, itemViewModelValidatorMock.Object);
            var result = await service.DeleteShoppingCartItemAsync(-1);
            Assert.IsFalse(result);
        }
    }
}