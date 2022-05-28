using System.ComponentModel.DataAnnotations.Schema;

namespace Term7MovieCore.Entities
{
    [Table("MovieActors")]
    public class MovieActor
    {
        public int MovieId { set; get; }
        public int ActorId { set; get; }
        public Movie Movie { set; get; }
        public Actor Actor { set; get; }
    }
}
