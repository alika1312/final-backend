using System.ComponentModel.DataAnnotations;

namespace api.Dtos
{
    public class AdminDto
    {


        public int userID { get; set; }
        public string username { get; set; } = string.Empty;
    }

    public class CreateAdminRequestDto
    {
        [Required]
        [MaxLength(50, ErrorMessage = "cannot exceed 50 characters")]
        [MinLength(1, ErrorMessage = "must have at least 1 character")]
        public int userID { get; set; }
    }

}
