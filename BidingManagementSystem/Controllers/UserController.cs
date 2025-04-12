using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Biding.Application.DTOs;
using Biding.Application.Repositories;
using Biding_management_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Biding.Application.IRepositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Biding_management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<User>();
        }

        // Register new user (post user)
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDTO userDTO)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(userDTO.Email);
            if (existingUser != null)
                return BadRequest("User with this email already exists.");

            // Convert the string Role to the UserRole enum --> all this logic just to achive RBAC :)
            if (!Enum.TryParse(userDTO.Role, true, out UserRole parsedRole))
            {
                return BadRequest("Invalid role specified. Accepted Roles are :User,ProcurementOfficer,Bidder,Evaluator");
            }

            // Create the User 
            var user = new User
            {
                Email = userDTO.Email,
                FullName = userDTO.FullName,
                PasswordHash = _passwordHasher.HashPassword(null, userDTO.Password),
                Role = parsedRole,
                CreatedAt = DateTime.UtcNow
            };

            // Add user to repository
            await _userRepository.AddUserAsync(user);
            return Ok("User registered successfully.");
        }


        // Login and return JWT token
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            var user = await _userRepository.GetUserByEmailAsync(userLoginDTO.Email);
            if (user == null)
                return Unauthorized("Invalid credentials.");

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, userLoginDTO.Password);
            if (passwordVerificationResult == PasswordVerificationResult.Failed)
                return Unauthorized("Invalid credentials.");

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        // Generate JWT token
        private string GenerateJwtToken(User user)
        {
            // Create claims
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                // Use the Role enum value (e.g., "Admin", "Bidder", etc.) note (its converted to string to ez save id database)
                new Claim(ClaimTypes.Role, user.Role.ToString()),  
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Create the key from appsettings.json (Jwt:Key)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            // Define signing credentials
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create the JWT token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),  
                signingCredentials: credentials
            );

            //Return the token as a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // To send the user token for resetting password (this is not the login token)
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] UserResetPasswordRequestDTO dto)
        {
            var token = await _userRepository.ForgotPasswordAsync(dto);
            if (token == null)
                return NotFound("User with that email doesn't exist.");

            // Simulate sending email--> like in real systems :)
            return Ok(new { Token = token });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] UserNewPasswordDTO dto)
        {
            var result = await _userRepository.ResetPasswordAsync(dto.Token, dto.NewPassword);
            if (!result)
                return BadRequest("Invalid or expired token");

            return Ok("Password reset successfully");
        }
    }
}
