using Core.Abstractions.Services;
using Core.Domain;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;
using System.Net;

namespace WebShop.Controllers
{
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UserController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet("user")]
        public List<UserViewModel> GetAllUsers()
        {
            return _usersService.GetAll();
        }

        [HttpPost("user")]
        public IActionResult Login(string userNameOrEMail, string password)
        {
            UserViewModel? userViewModel = _usersService.Login(userNameOrEMail, password);

            if(userViewModel == null) 
                return NotFound();

            HttpContext.Session.SetInt32("UserId", userViewModel.Id);

            return Ok();
        }

        //[HttpGet("products/search/{keyword}")]
        //public List<UserViewModel?> SearchByKeyword(string keyword)
        //{
        //    return _usersService.SearchByKeyWord(keyword);
        //}

        //[HttpGet("products/{productId}")]
        //public UserViewModel? GetById(int productId)
        //{
        //    return _usersService.GetById(productId);
        //}

        //[HttpDelete("products/{productId}")]
        //public bool DeleteById(int productId)
        //{
        //    return _usersService.Delete(productId);
        //}

        //[HttpPut("products")]
        //public bool UpdateProducts(int productId, UserViewModel UserViewModel)
        //{
        //    return _usersService.Update(productId, UserViewModel);
        //}
    }
}