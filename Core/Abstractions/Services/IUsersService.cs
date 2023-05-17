using Models.ViewModels;

namespace Core.Abstractions.Services
{
    public interface IUsersService
    {
        List<UserViewModel> GetAll();
        UserViewModel GetById(string id);
        bool Insert(UserViewModel user);
        bool Update(string userId, UserViewModel user);
        bool Delete(string id);
        UserViewModel GetUserByUsername(string username);
        UserViewModel GetUserByEmail(string email);
        UserViewModel? Login(string userNameOrEMail, string password);
    }
}
