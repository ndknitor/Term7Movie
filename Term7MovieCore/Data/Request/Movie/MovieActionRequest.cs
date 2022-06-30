

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Term7MovieCore.Data.Request.Movie
{
    public class MovieActionRequest
    {
        [Required]
        //[FromQuery(Name = "action")]
        public string Action { get; set; }
        //[FromQuery(Name = "movie-id")]
        public int MovieId { get; set; }
        //[FromQuery(Name = "page-size")]
        public int PageSize { get; set; } = 10;
        //[FromQuery(Name = "page-index")]
        public int PageIndex { get; set; } = 1;
        //[FromQuery(Name = "search-key")]
        public string SearchKey { get; set; } = "";
        [Range(-180, 180)]
        public double Longitude { get; set; }
        [Range(-90, 90)]
        //validate gòi mà
        public double Latitude { get; set; }
    }
}
