using System;
using System.Collections.Generic;
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
        public string professionName { get; set; } = string.Empty;
    }




}