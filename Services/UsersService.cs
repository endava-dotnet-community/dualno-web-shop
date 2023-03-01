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


namespace Services
{
    public class UsersService : IUsersService
    {
        private readonly IUserRepository _repository;

        public UsersService(IUserRepository repository)
        {
            _repository = repository;
        }

        public bool Delete(int id)
        {
            return _repository.Delete(id);
        }

        public List<UserViewModel> GetAll()
        {
            return _repository
                .GetAll()
                .Select<User?, UserViewModel?>(u => MapToViewModel(u))
                .ToList();
        }

        public UserViewModel GetById(int id)
        {
            return MapToViewModel(_repository.GetById(id));
        }

        public UserViewModel GetUserByEmail(string email)
        {
            return MapToViewModel(_repository.GetUserByEmail(email));
        }

        public UserViewModel GetUserByUsername(string username)
        {
            return MapToViewModel(_repository.GetUserByUsername(username));
        }

        public bool Insert(UserViewModel user)
        {
            return _repository.Insert(MapFromViewModel(user));
        }

        public bool Update(int userId, UserViewModel user)
        {
            return _repository.Update(userId, MapFromViewModel(user));
        }

        private UserViewModel? MapToViewModel(User? u)
        {
            if (u == null)
                return null;

            return new UserViewModel
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Password = String.Empty,
                UserName = u.UserName,
                Roles= u.Roles,
            };
        }

        private User? MapFromViewModel(UserViewModel? u)
        {
            if (u == null)
                return null;

            return new User
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Password = Encryption.Encrypt(u.Password),
                UserName = u.UserName,
                Roles = u.Roles,
            };
        }

        /// <summary>
        /// Login method to check usename/email and password
        /// </summary>
        /// <param name="userNameOrEMail">username or email</param>
        /// <param name="password">plain text password</param>
        /// <returns></returns>
        public UserViewModel? Login(string userNameOrEMail, string password)
        {
            bool isEmail = Regex.IsMatch(userNameOrEMail, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

            // check email or usename
            User user = isEmail ? 
                _repository.GetUserByEmail(userNameOrEMail) : 
                _repository.GetUserByUsername(userNameOrEMail);

            // not valid user
            if (user == null) 
                return null;

            // check password
            if(Encryption.Encrypt(password) != user.Password)
                return null;

            // convert to viewmodel
            return MapToViewModel(user);
        }
    }
}
