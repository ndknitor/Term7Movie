using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Term7MovieCore.Data.Request.Movie
{
    public class MovieHomePageRequest
    {
        [Required]
        public double Longtitude { get; set; }
        [Required]
        public double Latitude { get; set; }
    }
}
