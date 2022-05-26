using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Term7MovieCore.Entities
{
   public class Role : IdentityRole<long> 
   {
        public ICollection<UserRole> UserRoles { set; get; }
        [Column(TypeName = "nvarchar(30)")]
        public new string Name { get; set; }
        [Column(TypeName = "nvarchar(30)")]
        public new string NormalizedName { get; set; }
    }
}
