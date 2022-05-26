namespace Term7MovieCore.Entities
{
    public class MovieActor
    {
        public int MovieId { set; get; }
        public int ActorId { set; get; }
        public Movie Movie { set; get; }
        public Actor Actor { set; get; }
    }
}
