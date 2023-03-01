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
                UserViewModel? userViewModel = null;

                int? userId = HttpContext.Session.GetInt32("UserId");
                if (userId != null)
                    userViewModel = UsersService.GetById(userId.Value);
                
                if(userViewModel == null)
                {
                    userViewModel = new UserViewModel
                    {
                        Id = Int32.MinValue,
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
