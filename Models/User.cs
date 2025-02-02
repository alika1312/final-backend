using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public bool isCEO { get; set; }
        public int companyID { get; set; }
        public Company? Company { get; set; }
        public Admins? Admins { get; set; }
        public Branch? ManagedBranch { get; set; }
    }
}
