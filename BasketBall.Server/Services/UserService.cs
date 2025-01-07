using BasketBall.Server.Models;
using BasketBall.Server.Data;

namespace BasketBall.Server.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public User AddUser(string username, string password, string role = "viewer")
        {
            var user = new User
            {
                Username = username,
                Password = password,
                Role = role
            };

            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User? UpdateUserRole(int id, string newRole)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return null;

            user.Role = newRole;
            _context.SaveChanges();
            return user;
        }
        public bool UserExists(string username)
        {
            return _context.Users.Any(u => u.Username == username);
        }

    }
}