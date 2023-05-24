using Core.Abstractions.Repositories;
using Core.Abstractions.Services;
using Domain;
using Core.Util;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Models.Authentication;

namespace Services
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtService _jwtService;

        public UsersService(UserManager<IdentityUser> userManager, JwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        public bool Delete(string id)
        {
            return false;
        }

        public List<UserViewModel> GetAll()
        {
            return _userManager
                .Users
                .Select<IdentityUser, UserViewModel>(u => MapToViewModel(u))
                .ToList();
        }

        public UserViewModel GetById(string id)
        {
            IdentityUser identityUser = GetIdentityUser(id);

            IList<string> roles = _userManager.GetRolesAsync(identityUser).Result;

            return MapToViewModel(identityUser);
        }

        private IdentityUser GetIdentityUser(string id)
        {
            return _userManager.FindByIdAsync(id).Result;
        }

        public UserViewModel GetUserByEmail(string email)
        {
            return MapToViewModel(_userManager.FindByEmailAsync(email).Result);
        }

        public UserViewModel GetUserByUsername(string username)
        {
            return MapToViewModel(_userManager.FindByNameAsync(username).Result);
        }

        public bool Insert(UserViewModel user)
        {
            IdentityResult result = _userManager
                .CreateAsync(
                    new IdentityUser()
                    {
                        UserName = user.UserName,
                        Email = user.Email
                    },
                    user.Password)
                .Result;

            return result.Succeeded;
        }

        public bool Update(string userId, UserViewModel user)
        {
            IdentityUser identityUser = GetIdentityUser(userId);

            identityUser.Email = user.Email;

            IdentityResult result = _userManager
                .UpdateAsync(identityUser)
                .Result;

            return result.Succeeded;
        }

        private static UserViewModel MapToViewModel(IdentityUser u)
        {
            if (u == null)
                return null;

            return new UserViewModel
            {
                Id = u.Id,
                FirstName = "",
                LastName = "",
                Email = u.Email,
                Password = String.Empty,
                UserName = u.UserName,
                Roles = new List<UserRole>(),
            };
        }

        /// <summary>
        /// Login method to check usename/email and password
        /// </summary>
        /// <param name="userNameOrEMail">username or email</param>
        /// <param name="password">plain text password</param>
        /// <returns></returns>
        public UserViewModel Login(string userNameOrEMail, string password)
        {
            bool isEmail = Regex.IsMatch(userNameOrEMail, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

            // check email or usename
            IdentityUser identityUser = isEmail ?
                _userManager.FindByEmailAsync(userNameOrEMail).Result :
                _userManager.FindByNameAsync(userNameOrEMail).Result;

            // not valid user
            if (identityUser == null)
                return null;

            // check password
            if (!_userManager.CheckPasswordAsync(identityUser, password).Result)
                return null;

            // convert to viewmodel
            return MapToViewModel(identityUser);
        }


        public AuthenticationResponse CreateToken(string userName)
        {
            return _jwtService.CreateToken(
                _userManager.FindByNameAsync(userName).Result);
        }
    }
}
