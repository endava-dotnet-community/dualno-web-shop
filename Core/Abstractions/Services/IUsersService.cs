using Models.ViewModels;

namespace Core.Abstractions.Services
{
    public interface IUsersService
    {
        List<UserViewModel> GetAll();
        UserViewModel GetById(int id);
        bool Insert(UserViewModel user);
        bool Update(int userId, UserViewModel user);
        bool Delete(int id);
        UserViewModel GetUserByUsername(string username);
        UserViewModel GetUserByEmail(string email);
        UserViewModel? Login(string userNameOrEMail, string password);
    }
}
