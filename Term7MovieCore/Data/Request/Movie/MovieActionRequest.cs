

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Term7MovieCore.Data.Request.Movie
{
    public class MovieActionRequest
    {
        [Required]
        public string Action { get; set; }
        public int? MovieId { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 1;
        public string SearchKey { get; set; } = "";
        [Range(-180, 180)]
        public double? Longitude { get; set; }
        [Range(-90, 90)]
        public double? Latitude { get; set; }
        public bool IsAvailableOnly { get; set; } = false;
        public bool IsDisabledOnly { set; get; } = false;
    }
}
