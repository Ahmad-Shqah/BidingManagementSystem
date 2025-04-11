

namespace Biding.Application.DTOs
{
    public class UserNewPasswordDTO
    {
        //for reset password controller
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
