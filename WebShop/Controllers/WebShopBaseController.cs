using Core.Abstractions.Services;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;

namespace WebShop.Controllers
{
    public class WebShopBaseController : ControllerBase
    {
        public readonly IUsersService UsersService;

        public WebShopBaseController(IUsersService usersService)
        {
            UsersService = usersService;
        }

        public UserViewModel CurrentUser
        {
            get
            {
                UserViewModel userViewModel = 
                    UsersService.GetUserByUsername(this.Request.HttpContext.User.Identity.Name);
                
                if(userViewModel == null)
                {
                    userViewModel = new UserViewModel
                    {
                        Id = "",
                        UserName = "UnknownUser",
                        Roles = new List<UserRole>
                        {
                            UserRole.None
                        }
                    };
                }

                return userViewModel;
            }
        }

    }
}
