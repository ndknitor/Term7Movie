

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Term7MovieCore.Data.Request
{
    public class UserRequest
    {
        [Required]
        public int UserId { get; set; }
        public string FullName { get; set; }
    }
}
