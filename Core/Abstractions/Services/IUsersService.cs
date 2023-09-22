using Models.Authentication;
using Models.ViewModels;

namespace Core.Abstractions.Services
{
    public interface IUsersService
    {
        List<UserViewModel> GetAll();
        Task<UserViewModel> GetById(string id);
        Task<bool> Insert(UserViewModel user);
        Task<bool> Update(string userId, UserViewModel user);
        Task<bool> Delete(string id);
        Task<UserViewModel> GetUserByUsername(string username);
        Task<UserViewModel> GetUserByEmail(string email);
        Task<UserViewModel> Login(string userNameOrEMail, string password);
        Task Logout();
        Task<AuthenticationResponse> CreateToken(string userName);
    }
}
