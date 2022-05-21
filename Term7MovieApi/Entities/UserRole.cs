using Microsoft.AspNetCore.Identity;

namespace Term7MovieApi.Entities
{
    public class UserRole : IdentityUserRole<long> 
    { 
        public Role Role { set; get; }
        public User User { set; get; }
    }
}
