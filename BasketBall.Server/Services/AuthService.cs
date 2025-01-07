using BasketBall.Server.Models;
using BasketBall.Server.Data;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace BasketBall.Server.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly string _secretKey;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _secretKey = configuration["Jwt:SecretKey"];
            if (string.IsNullOrEmpty(_secretKey))
            {
                throw new ArgumentNullException("Jwt:SecretKey is not configured.");
            }
        }


        public string? Authenticate(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Username and password cannot be null or empty.");
            }

            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            if (string.IsNullOrEmpty(user.Role))
            {
                throw new InvalidOperationException("User role is not defined.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey ?? throw new ArgumentNullException("SecretKey"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
            {
                new System.Security.Claims.Claim("id", user.Id.ToString()),
                new System.Security.Claims.Claim("role", user.Role)
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };



            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}