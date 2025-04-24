using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Branch
    {
        public int branchID { get; set; }
        public string branchName { get; set; } = string.Empty;
        public int? ManagerID { get; set; }
        public ApplicationUser? Manager { get; set; }
        public ICollection<Worker> workers { get; set; } = new List<Worker>();
    }
}