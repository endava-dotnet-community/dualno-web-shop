using Core.Abstractions.Services;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;
using Services.Exceptions;

namespace WebShop.Controllers
{
    [Route("api")]
    [ApiController]
    public class CategoryController : WebShopBaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService, IUsersService usersService) 
            : base(usersService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("category")]
        public List<CategoryViewModel> GetAllCategory()
        {
            return _categoryService.GetAllCategories();
        }

        [HttpPost("category")]
        public IActionResult Insert([FromBody] CategoryViewModel productModel)
        {
            if (!CurrentUser.Roles.Contains(UserRole.Administrator))
            {
                throw new NotAuthorizedException();
            }

            _categoryService.Insert(productModel);
            return Ok();
        }

        [HttpGet("category/search/{keyword}")]
        public List<CategoryViewModel> SearchByKeyword(string keyword)
        {
            return _categoryService.SearchByKeyWord(keyword);
        }

        [HttpGet("category/{productId}")]
        public CategoryViewModel GetById(int productId)
        {
            return _categoryService.GetById(productId);
        }

        [HttpDelete("category/{productId}")]
        public bool DeleteById(int productId)
        {
            if (!CurrentUser.Roles.Contains(UserRole.Administrator))
            {
                throw new NotAuthorizedException();
            }

            return _categoryService.Delete(productId);
        }

        [HttpPut("category")]
        public bool UpdateCategory(int productId, CategoryViewModel productViewModel)
        {
            if (!CurrentUser.Roles.Contains(UserRole.Administrator))
            {
                throw new NotAuthorizedException();
            }

            return _categoryService.Update(productId, productViewModel);
        }
    }
}