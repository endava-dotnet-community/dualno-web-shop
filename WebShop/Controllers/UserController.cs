using Core.Abstractions.Services;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;
using System.Net;
using Services.Exceptions;

namespace WebShop.Controllers
{
    [Route("api")]
    [ApiController]
    public class UserController : WebShopBaseController
    {
        public UserController(IUsersService usersService) 
            : base(usersService)
        {
        }

        [HttpGet("users")]
        public List<UserViewModel> GetAllUsers()
        {
            if(!CurrentUser.Roles.Contains(UserRole.Administrator))
            {
                throw new NotAuthorizedException();
            }

            return UsersService.GetAll();
        }

        [HttpPost("user/login")]
        public IActionResult Login(string userNameOrEMail, string password)
        {
            UserViewModel userViewModel = UsersService.Login(userNameOrEMail, password);

            if (userViewModel == null)
                return NotFound();

            HttpContext.Session.SetString("UserId", userViewModel.Id);

            return Ok();
        }

        [HttpGet("user/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Ok();
        }

    }
}