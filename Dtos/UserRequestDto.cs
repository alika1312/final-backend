using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class UserRequestDto
    {
        [Required]
        [MaxLength(50, ErrorMessage = "cannot exceed 50 characters")]
        [MinLength(1, ErrorMessage = "must have at least 1 character")]
        [RegularExpression(@"^[\p{L}\p{N}]+$", ErrorMessage = "Username can only contain letters or digits from any language.")]
        public string username { get; set; } = string.Empty;
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Password must be between 8 and 15 characters and contain one uppercase letter, one lowercase letter, one digit and one special character.")]

        public string password { get; set; } = string.Empty;
        [Required]
        public bool isCEO { get; set; }
        [Required]
        public int companyID { get; set; }

    }
}