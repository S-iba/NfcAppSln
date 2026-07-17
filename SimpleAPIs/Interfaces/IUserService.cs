using SimpleAPIs.Models;

namespace SimpleAPIs.Interfaces
{
    public interface IUserService
    {
        List<User> GetAllUsers();
        User? CreateUser(User user);
        User? EditUser(User user);
        bool UserExists(int userId);
        User? GetUserById(int userId);
        bool DeleteUser(int userId);
    }
}
