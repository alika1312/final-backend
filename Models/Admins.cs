using System;

namespace api.Models
{
    public class Admins
    {
        public int userID { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
