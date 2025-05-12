using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class BranchDto
    {
        public int branchID { get; set; }
        public string branchName { get; set; } = string.Empty;
        public int? ManagerID { get; set; }
    }
        public class BranchNameDto
    {
        public string branchName { get; set; } = string.Empty;
    }

}