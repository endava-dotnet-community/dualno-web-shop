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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        //private readonly JwtService _jwtService;

        public UsersService(
            RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager
            //JwtService jwtService
            )
        {
            _roleManager = roleManager;
            _signInManager = signInManager;
            //_jwtService = jwtService;
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<UserViewModel>> GetAll()
        {
            List<UserViewModel> users = new List<UserViewModel>();
            foreach (var identityUser in _signInManager.UserManager.Users.ToList())
            {
                users.Add(MapToViewModel(identityUser, await GetRolesForIdentityUser(identityUser)));
            }

            return users;
        }

        public async Task<UserViewModel> GetById(string id)
        {
            IdentityUser identityUser = await GetIdentityUser(id);
            return MapToViewModel(identityUser, await GetRolesForIdentityUser(identityUser));
        }

        private async Task<IdentityUser> GetIdentityUser(string id)
        {
            return await _signInManager.UserManager.FindByIdAsync(id);
        }

        public async Task<UserViewModel> GetUserByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                // Handle the case where the email is null or empty
                // You might want to throw an exception or return an appropriate response
                throw new Exception($"User with email {email} does not exist!");
            }

            IdentityUser identityUser = await _signInManager.UserManager.FindByEmailAsync(email);
            return MapToViewModel(identityUser, await GetRolesForIdentityUser(identityUser));
        }

        public async Task<UserViewModel> GetUserByUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                // Handle the case where the username is null or empty
                // You might want to throw an exception or return an appropriate response
                throw new Exception($"User with email {username} does not exist!");
            }

            IdentityUser identityUser = await _signInManager.UserManager.FindByNameAsync(username);
            return MapToViewModel(identityUser, await GetRolesForIdentityUser(identityUser));
        }

        public async Task<bool> Insert(UserViewModel user)
        {
            IdentityResult result = await _signInManager.UserManager
                .CreateAsync(
                    new IdentityUser()
                    {
                        UserName = user.UserName,
                        Email = user.Email
                    },
                    user.Password);

            if (!result.Succeeded)
                return false;

            IdentityUser identityUser = _signInManager.UserManager.Users.FirstOrDefault(u => u.UserName.Equals(user.UserName));
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

                var addToRoleResult = await _signInManager.UserManager.AddToRoleAsync(identityUser, role.ToString());
            }

            return result.Succeeded;
        }

        public async Task<bool> Update(string id, UserViewModel user)
        {
            IdentityUser identityUser = await GetIdentityUser(id);

            identityUser.Email = user.Email;

            IdentityResult result = await _signInManager.UserManager.UpdateAsync(identityUser);

            if (!result.Succeeded)
                return false;

            // Remove the user from all roles
            foreach (UserRole role in await GetRolesForIdentityUser(identityUser))
            {
                await _signInManager.UserManager.RemoveFromRoleAsync(identityUser, role.ToString());
            }

            // Add the user to the selected roles
            foreach (UserRole role in user.Roles)
            {
                await _signInManager.UserManager.AddToRoleAsync(identityUser, role.ToString());
            }

            return true;
        }

        private async Task<List<UserRole>> GetRolesForIdentityUser(IdentityUser identityUser)
        {
            List<UserRole> userRoles = new List<UserRole>();
            IList<string> roles = await _signInManager.UserManager.GetRolesAsync(identityUser);

            foreach (UserRole userRole in Enum.GetValues(typeof(UserRole)))
            {
                if (roles.Any(r => r.Equals(userRole.ToString(), StringComparison.InvariantCultureIgnoreCase)))
                    userRoles.Add(userRole);
            }

            return userRoles;
        }

        private static UserViewModel MapToViewModel(IdentityUser u, List<UserRole> roles)
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
                Roles = roles,
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
                await _signInManager.UserManager.FindByEmailAsync(userNameOrEMail) :
                await _signInManager.UserManager.FindByNameAsync(userNameOrEMail);

            // not valid user
            if (identityUser == null)
                return null;

            // check password
            if (!await _signInManager.UserManager.CheckPasswordAsync(identityUser, password))
                return null;

            await _signInManager.SignInAsync(identityUser, false);

            // convert to viewmodel
            return MapToViewModel(identityUser, await GetRolesForIdentityUser(identityUser));
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<AuthenticationResponse> CreateToken(string userName)
        {
            throw new NotImplementedException();
            //return _jwtService.CreateToken(await _signInManager.UserManager.FindByNameAsync(userName));
        }
    }
}
