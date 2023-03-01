using Domain;

namespace Core.Abstractions.Repositories
{
    public interface IUserRepository
    {
        List<User> GetAll();
        User GetById(int id);
        bool Insert(User user);
        bool Update(int userId, User user);
        bool Delete(int id);
        User GetUserByUsername(string username);
        User GetUserByEmail(string email);

    }
}
