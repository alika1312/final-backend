using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Company
    {
        public int companyID { get; set; }
        public string companyName { get; set; } = string.Empty;

        public ICollection<ApplicationUser> users { get; set; } = new List<ApplicationUser>();
        public ICollection<ShiftType> shiftTypes { get; set; } = new List<ShiftType>();
        public ICollection<Profession> professions { get; set; } = new List<Profession>();
    }
}