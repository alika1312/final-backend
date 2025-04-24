using System.ComponentModel.DataAnnotations;

namespace api.Dtos
{
    public class ChangePasswordDto
    {
        [Required]

        public string CurrentPassword { get; set; } = string.Empty;
        [Required]

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Password must be between 8 and 15 characters and contain one uppercase letter, one lowercase letter, one digit and one special character.")]

        public string NewPassword { get; set; } = string.Empty;
    }
}
