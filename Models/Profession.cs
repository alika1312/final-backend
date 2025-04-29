using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Profession
    {
        public int professionID { get; set; }
        public string professionName { get; set; } = string.Empty;

        
        public int companyID { get; set; }
        public Company Company { get; set; }
        public ICollection<WorkerProfession> WorkerProfessions { get; set; } = new List<WorkerProfession>();

    }
}