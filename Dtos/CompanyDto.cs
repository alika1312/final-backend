using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class CompanyDto
    {
        public int companyID { get; set; }
        public string companyName { get; set; } = string.Empty;

        public List<string> userNames { get; set; } = new List<string>();
    }
}