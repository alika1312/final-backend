using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class UserRequestDto
    {
        public string username { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public bool isCEO { get; set; }
        public int companyID { get; set; }

    }
}