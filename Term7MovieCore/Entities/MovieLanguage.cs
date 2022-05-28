namespace Term7MovieCore.Entities
{
    public class MovieLanguage
    {
        public int MovieId { get; set; }
        public int LanguageId { set; get; }
        public Movie Movie { get; set; }
        public Language Language { get; set; }
    }
}
