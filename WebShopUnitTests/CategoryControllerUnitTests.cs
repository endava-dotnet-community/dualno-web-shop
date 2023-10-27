using Core.Abstractions.Services;
using Domain;
using Models.ViewModels;
using Moq;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Controllers;

namespace WebShopUnitTests
{
    [TestClass]
    public class CategoryControllerUnitTests
    {
        [TestMethod]
        public void InsertAsAdministratorTestMethod()
        {
            var categoryViewModel = new CategoryViewModel()
            {
                Name = "Test"
            };

            var categoryServiceMock = new Mock<ICategoryService>();
            categoryServiceMock
                .Setup(service => service.InsertAsync(categoryViewModel))
                .Returns(Task.CompletedTask);

            var userServiceMock = new Mock<IUsersService>();
            userServiceMock
                .Setup(service => service.GetUserByUsername(It.IsAny<string>()))
                .Returns(Task.FromResult(new UserViewModel()
                {
                    UserName = "Admin",
                    Roles = new List<UserRole>() { UserRole.Administrator }
                }));

            var controller = new CategoryController(categoryServiceMock.Object, userServiceMock.Object);


            //Act

            controller.IdentityName = "Admin";

            var result = controller.Insert(categoryViewModel);


            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Task));

            Assert.IsTrue(result.IsCompleted);
            Assert.IsNull(result.Exception);
        }

        [TestMethod]
        public void InsertAsUnknowUserTestMethod()
        {
            var categoryViewModel = new CategoryViewModel()
            {
                Name = "Test"
            };

            var categoryServiceMock = new Mock<ICategoryService>();
            categoryServiceMock
                .Setup(service => service.InsertAsync(categoryViewModel))
                .Returns(Task.CompletedTask);

            var userServiceMock = new Mock<IUsersService>();
            userServiceMock
                .Setup(service => service.GetUserByUsername(It.IsAny<string>()))
                .Returns(Task.FromResult((UserViewModel)null));

            var controller = new CategoryController(categoryServiceMock.Object, userServiceMock.Object);


            //Act

            controller.IdentityName = "Admin";

            var result = controller.Insert(categoryViewModel);


            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Task));

            Assert.IsTrue(result.IsCompleted);
            Assert.IsTrue(result.Exception.InnerExceptions[0].GetType() == typeof(NotAuthorizedException));
        }
    }
}
