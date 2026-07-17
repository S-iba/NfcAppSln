using SimpleAPIs.Data;
using SimpleAPIs.Interfaces;
using SimpleAPIs.Models;

namespace SimpleAPIs.Services
{
    public class UserService : IUserService
    {
       private readonly UserDbContext _context;
        public UserService(UserDbContext context)
        {
            _context = context;
        }

        public User? CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return GetUserById(user.UserId) ?? user;
        }

        public User? GetUserById(int userId)
        {
             User? user = _context.Users.Find(userId);
            return user;
        }

        public User? EditUser(User user)
        {
            User? existingUser = _context.Users.Find(user.UserId);

            if (existingUser == null)
            {
                return null;
            }

            existingUser.Username = user.Username;
            existingUser.UUID = user.UUID;

            _context.SaveChanges();
            return existingUser;
        }

        public List<User> GetAllUsers()
        {
            List<User> users = _context.Users.ToList();

            return users;
        }

        public bool UserExists(int userId)
        {
            return _context.Users.Any(u => u.UserId == userId);
        }

        public bool DeleteUser(int userId)
        {
            User? user = _context.Users.Find(userId);

            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            _context.SaveChanges();
            
            return true;
        }
    }
}
