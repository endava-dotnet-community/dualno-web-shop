using Core.Abstractions.Services;
using Domain;
using Models.ViewModels;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Models.Authentication;

namespace Services
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtService _jwtService;

        public UsersService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, JwtService jwtService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public List<UserViewModel> GetAll()
        {
            return _userManager
                .Users
                .Select<IdentityUser, UserViewModel>(u => MapToViewModel(u).Result)
                .ToList();
        }

        public async Task<UserViewModel> GetById(string id)
        {
            IdentityUser identityUser = await GetIdentityUser(id);

            IList<string> roles = await _userManager.GetRolesAsync(identityUser);

            return await MapToViewModel(identityUser);
        }

        private async Task<IdentityUser> GetIdentityUser(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<UserViewModel> GetUserByEmail(string email)
        {
            return await MapToViewModel(await _userManager.FindByEmailAsync(email));
        }

        public async Task<UserViewModel> GetUserByUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                // Handle the case where the username is null or empty
                // You might want to throw an exception or return an appropriate response
            }
            return await MapToViewModel(await _userManager.FindByNameAsync(username));
        }

        public async Task<bool> Insert(UserViewModel user)
        {
            IdentityResult result = await _userManager
                .CreateAsync(
                    new IdentityUser()
                    {
                        UserName = user.UserName,
                        Email = user.Email
                    },
                    user.Password);

            if (!result.Succeeded)
                return false;

            IdentityUser identityUser = _userManager.Users.FirstOrDefault(u => u.UserName.Equals(user.UserName));
            foreach (UserRole role in user.Roles)
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    // Role doesn't exist, you can create it here
                    var newRole = new IdentityRole(role.ToString());
                    var roleResult = await _roleManager.CreateAsync(newRole);
                    if (!roleResult.Succeeded)
                    {
                        return false;
                    }
                }

                var addToRoleResult = await _userManager.AddToRoleAsync(identityUser, role.ToString());
            }

            return result.Succeeded;
        }

        public async Task<bool> Update(string id, UserViewModel user)
        {
            IdentityUser identityUser = await GetIdentityUser(id);

            identityUser.Email = user.Email;

            IdentityResult result = await _userManager.UpdateAsync(identityUser);

            if (!result.Succeeded)
                return false;

            // Remove the user from all roles
            foreach (UserRole role in await GetRolesForIdentityUser(identityUser))
            {
                await _userManager.RemoveFromRoleAsync(identityUser, role.ToString());
            }

            // Add the user to the selected roles
            foreach (UserRole role in user.Roles)
            {
                await _userManager.AddToRoleAsync(identityUser, role.ToString());
            }

            return true;
        }

        private async Task<List<UserRole>> GetRolesForIdentityUser(IdentityUser user)
        {
            List<UserRole> roles = new List<UserRole>();
            foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
            {
                if (await _userManager.IsInRoleAsync(user, role.ToString()))
                {
                    roles.Add(role);
                }

            }

            return roles;
        }

        private async Task<UserViewModel> MapToViewModel(IdentityUser u)
        {
            if (u == null)
                return null;

            var userViewModel = new UserViewModel
            {
                Id = u.Id,
                FirstName = "",
                LastName = "",
                Email = u.Email,
                Password = String.Empty,
                UserName = u.UserName,
                Roles = await GetRolesForIdentityUser(u),
            };

            return userViewModel;
        }


        /// <summary>
        /// Login method to check usename/email and password
        /// </summary>
        /// <param name="userNameOrEMail">username or email</param>
        /// <param name="password">plain text password</param>
        /// <returns></returns>
        public async Task<UserViewModel> Login(string userNameOrEMail, string password)
        {
            bool isEmail = Regex.IsMatch(userNameOrEMail, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

            // check email or usename
            IdentityUser identityUser = isEmail ?
                await _userManager.FindByEmailAsync(userNameOrEMail) :
                await _userManager.FindByNameAsync(userNameOrEMail);

            // not valid user
            if (identityUser == null)
                return null;

            // check password
            if (!await _userManager.CheckPasswordAsync(identityUser, password))
                return null;

            // convert to viewmodel
            return await MapToViewModel(identityUser);
        }

        public async Task<AuthenticationResponse> CreateToken(string userName)
        {
            return _jwtService.CreateToken(await _userManager.FindByNameAsync(userName));
        }
    }
}
