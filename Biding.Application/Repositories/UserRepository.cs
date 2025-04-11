using System.Threading.Tasks;
using Biding_management_System.Models;
using Biding.Application.IRepositories;
using Biding.Application.DTOs;
using Biding_management_System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;


namespace Biding.Application.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly SystemDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;

        public UserRepository(SystemDbContext context, IConfiguration configuration) : base(context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
            _configuration = configuration;
        }
        //get the user with this email (for login)
        public async Task<User> GetUserByEmailAsync(string email)
        {
            var value = _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            return await value;
        }
        //add new user (for register)
        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        //handling the forgot password logic
        public async Task<string?> ForgotPasswordAsync(UserResetPasswordRequestDTO dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, user.Email) }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token); 
        }
        public async Task<bool> ResetPasswordAsync(string token, string newPassword)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out _);

                var email = principal.FindFirst(ClaimTypes.Email)?.Value;
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

                if (user == null) return false;

                user.PasswordHash = _passwordHasher.HashPassword(user, newPassword);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
               public User GetUserById(int id)
        {
            var user = _context.Users.Where(u => u.Id == id).FirstOrDefault();
            return user;
        }
    }
}
