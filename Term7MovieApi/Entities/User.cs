﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace Term7MovieApi.Entities
{
    public class User : IdentityUser<long>
    {
        [ProtectedPersonalData, Column(TypeName = "char(10)")]
        public new string PhoneNumber { get; set; }
        [PersonalData, Column(TypeName = "varchar(200)")]
        public new string NormalizedEmail { get; set; }
        [ProtectedPersonalData, Column(TypeName = "varchar(200)")]
        public new string Email { get; set; }
        [Column(TypeName = "varchar(200)")]
        public new string NormalizedUserName { get; set; }
        [ProtectedPersonalData, Column(TypeName = "varchar(200)")]
        public new string UserName { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string PictureUrl { set; get; }
        public int? Point { set; get; }
        public int? TheaterId { set; get; }
        [Required]
        public int StatusId { set; get; }
        public UserStatus Status {set; get;}
        public ICollection<UserRole> UserRoles { set; get; }
        [JsonIgnore]
        public ICollection<RefreshToken> RefreshTokens { set; get; }
    }
}
