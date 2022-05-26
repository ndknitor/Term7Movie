using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Term7MovieCore.Entities
{
    public class UserStatus
    {
        public int Id { set; get; }
        [Column(TypeName = "nvarchar(30)")]
        public string Name { set; get; }
        public ICollection<User> Users { set; get; }
    }
}
