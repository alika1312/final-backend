using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class CompanyRequestDto
    {
        [Required]
        [MaxLength(50, ErrorMessage = "cannot exceed 50 characters")]
        [MinLength(1, ErrorMessage = "must have at least 1 character")]
        public string companyName { get; set; } = string.Empty;
    }
}