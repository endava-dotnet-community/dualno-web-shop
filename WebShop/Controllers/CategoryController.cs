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
        public async Task<List<CategoryViewModel>> GetAllCategory()
        {
            return await _categoryService.GetAllCategoriesAsync();
        }

        [HttpPost("category")]
        public async Task<IActionResult> Insert([FromBody] CategoryViewModel productModel)
        {
            if (!CurrentUser.Roles.Contains(UserRole.Administrator))
            {
                throw new NotAuthorizedException();
            }

            await _categoryService.InsertAsync(productModel);
            return Ok();
        }

        [HttpGet("category/search/{keyword}")]
        public async Task<List<CategoryViewModel>> SearchByKeyword(string keyword)
        {
            return await _categoryService.SearchByKeyWordAsync(keyword);
        }

        [HttpGet("category/{productId}")]
        public async Task<CategoryViewModel> GetById(int productId)
        {
            return await _categoryService.GetByIdAsync(productId);
        }

        [HttpDelete("category/{productId}")]
        public async Task<bool> DeleteById(int productId)
        {
            if (!CurrentUser.Roles.Contains(UserRole.Administrator))
            {
                throw new NotAuthorizedException();
            }

            return await _categoryService.DeleteAsync(productId);
        }

        [HttpPut("category")]
        public async Task<bool> UpdateCategory(int productId, CategoryViewModel productViewModel)
        {
            if (!CurrentUser.Roles.Contains(UserRole.Administrator))
            {
                throw new NotAuthorizedException();
            }

            return await _categoryService.UpdateAsync(productId, productViewModel);
        }
    }
}