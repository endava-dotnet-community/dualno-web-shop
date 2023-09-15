using Core.Abstractions.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Services.Exceptions;
using WebShop.Authorization.Requirements;

namespace WebShop.Authorization.Handlers
{
    public class RequireAdminHandler : AuthorizationHandler<AdminRoleRequirement>
    {
        private readonly IUsersService _usersService;
        private readonly HttpContext _httpContext;

        public RequireAdminHandler(
            IUsersService usersService,
            IHttpContextAccessor httpContextAccessor)
        {
            _usersService = usersService;
            _httpContext = httpContextAccessor.HttpContext;

            ArgumentNullException.ThrowIfNull(_httpContext);
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context, AdminRoleRequirement requirement)
        {
            var userId = _httpContext.Session.GetString("UserId");

            if (userId == null)
                throw new NotAuthorizedException();

            var loggedInUser = await _usersService.GetById(userId);

            foreach (var role in loggedInUser.Roles)
            {
                if (role == UserRole.Administrator)
                {
                    context.Succeed(requirement);
                    return;
                }
            }

            throw new NotAuthorizedException();
        }
    }
}