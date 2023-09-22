using Core.Abstractions.Services;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;
using System.Net;
using Services.Exceptions;
using Microsoft.AspNetCore.Identity;
using Models.Authentication;
using Services;
using Microsoft.AspNetCore.Authorization;

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

        [Authorize]
        [HttpGet("users")]
        public List<UserViewModel> GetAllUsers()
        {
            if(!CurrentUser.Roles.Contains(UserRole.Administrator))
            {
                throw new NotAuthorizedException();
            }

            return UsersService.GetAll();
        }

        [HttpPost("user/create")]
        public async Task<IActionResult> Create(UserViewModel userViewModel)
        {
            bool result = await UsersService.Insert(userViewModel);

            if (!result)
                return BadRequest(StatusCodes.Status500InternalServerError);

            return Ok();
        }

        [HttpPost("user/login")]
        public async Task<IActionResult> Login(string userNameOrEMail, string password)
        {
            UserViewModel userViewModel = await UsersService.Login(userNameOrEMail, password);

            if (userViewModel == null)
                return NotFound();

            HttpContext.Session.SetString("UserId", userViewModel.Id);

            return Ok();
        }

        [HttpGet("user/logout")]
        public async Task<IActionResult> Logout()
        {
            await UsersService.Logout();
            HttpContext.Session.Clear();
            return Ok();
        }

        // POST: api/Users/BearerToken
        [HttpPost("BearerToken")]
        public async Task<ActionResult<AuthenticationResponse>> CreateBearerToken(AuthenticationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Bad credentials");
            }

            UserViewModel user = await UsersService.Login(request.UserName, request.Password);

            if (user == null)
            {
                return BadRequest("Bad credentials");
            }

            var token = await UsersService.CreateToken(user.UserName);

            return Ok(token);
        }

    }
}