using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Dtos
{
    public class ShiftTypeDto
    {
        public int shiftTypeID { get; set; }
        public string shiftTypeName { get; set; } = string.Empty;


    }
}
