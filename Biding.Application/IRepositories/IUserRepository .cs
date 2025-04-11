using Biding.Application.DTOs;
using Biding_management_System.Models;

namespace Biding.Application.IRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        //for login logic
       Task<User> GetUserByEmailAsync(string email);
        //fpr register logic
        Task AddUserAsync(User user);

        //for reseting password logic
        Task<string?> ForgotPasswordAsync(UserResetPasswordRequestDTO email);
        Task<bool> ResetPasswordAsync(string token, string newPassword);

        //for tender and bid authorization stuff 
        User GetUserById(int id);
    }
}
