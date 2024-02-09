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
    public class ShoppingCartServiceUnitTests
    {

        //[TestMethod]
        //public void GetAllCategoriesAsyncTestMethod()
        //{
        //    var repostitoryMock = new Mock<ICategoryRepository>();

        //    var viewModelValidatorMock = new Mock<IValidator<CategoryViewModel>>();

        //    repostitoryMock
        //        .Setup(repos => repos.GetAllCategoriesAsync())
        //        .Returns(GetCategories());

        //    var service = new CategoryService(repostitoryMock.Object, viewModelValidatorMock.Object);

        //    var result = service.GetAllCategoriesAsync();

        //    Assert.IsNotNull(result?.Result);
        //    Assert.AreEqual(3, result.Result.Count);
        //    Assert.IsInstanceOfType(result, typeof(Task<List<CategoryViewModel>>));
        //}

    }
}