

namespace Term7MovieCore.Data.Dto
{
    public class MovieDetailDTO : MovieModelDto //hehe why not
    {
        public IEnumerable<MovieType> movieTypes { get; set; }
    }
}
