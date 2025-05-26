using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class WorkerDto
    {
        public int workerID { get; set; }
        public string workerName { get; set; } = string.Empty;
    }

    public class CreateWorkerDto
    {
        [Required]
        [MaxLength(50, ErrorMessage = "cannot exceed 50 characters")]
        [MinLength(1, ErrorMessage = "must have at least 1 character")]
        public string workerName { get; set; } = string.Empty;
        [Required]
        public int branchID { get; set; } 
        
    }
    public class CreateWorkerWithProfessionsDto
{
    public string? workerName { get; set; }
    public int branchID { get; set; }
    public List<int>? professionIDs { get; set; }
}


}