using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class ProfessionDto
    {
        public int professionID { get; set; }
        public string professionName { get; set; } = string.Empty;
    }

    public class CreateProfessionDto
    {
        [Required]
        [MaxLength(50, ErrorMessage = "cannot exceed 50 characters")]
        [MinLength(1, ErrorMessage = "must have at least 1 character")]
        public string professionName { get; set; } = string.Empty;
    }




}