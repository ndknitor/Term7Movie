using Microsoft.AspNetCore.Identity;

namespace Term7MovieCore.Entities
{
    public class UserRole : IdentityUserRole<long> 
    { 
        public Role Role { set; get; }
        public User User { set; get; }
    }
}
