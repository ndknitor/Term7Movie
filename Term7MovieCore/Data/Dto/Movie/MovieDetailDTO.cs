namespace Term7MovieCore.Data.Dto.Movie
{
    public class MovieDetailDTO// :MovieModelDto //hehe why not, well chỉ vì muốn format date thôi :v
    {
        public MovieDetailDTO()
        {
            Random random = new Random();
            Director = GetRandomDirector(random.Next(0, 11));
            Actors = new string[5];
            for(int i = 0; i < 5; i++)
            {
                Actors[i] = GetRandomActors(random.Next(0, 22));
            }
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string ReleaseDate { set; get; }
        public int Duration { set; get; }
        public int? RestrictedAge { set; get; }
        public string PosterImageUrl { set; get; }
        public string CoverImageUrl { set; get; }
        public string TrailerUrl { set; get; }
        public string Description { set; get; }
        public long ViewCount { set; get; }
        public float TotalRating { set; get; }
        public string Director { get; set; }
        public string[] Actors { get; set; }
        public IEnumerable<MovieType> movieTypes { get; set; }

        private string GetRandomDirector(int choosen)
        {
            switch(choosen)
            {
                case 1: return "Brad Bird";
                case 2: return "Pete Docter";
                case 3: return "Michael Bay";
                case 4: return "Christopher Nolan";
                case 5: return "Ridley Scott";
                case 6: return "Tim Burton";
                case 7: return "Robert Zemeckis";
                case 8: return "David Cronenberg";
                case 9: return "Danny Boyle";
                case 10: return "Oliver Stone";
                default: return "Phan Đình Thiên Ân";
            }
        }

        private string GetRandomActors(int choosen)
        {
            switch(choosen)
            {
                case 1: return "Tom Hanks";
                case 2: return "Will Smith";
                case 3: return "Samuel L. Jackson";
                case 4: return "Denzel Washington";
                case 5: return "Tom Cruise";
                case 6: return "Johnny Depp";
                case 7: return "Leonardo DiCaprio";
                case 8: return "Morgan Freeman";
                case 9: return "Robert De Niro";
                case 10: return "Anthony Hopkins";
                case 11: return "Harrison Ford";
                case 12: return "Al Pacino";
                case 13: return "Jack Nicholson";
                case 14: return "Sandra Bullock";
                case 15: return "Joaquin Phoenix";
                case 16: return "Amitabh Bachchan";
                case 17: return "Akshay Kumar";
                case 18: return "Robert Downey Jr.";
                case 19: return "Don Cheadle";
                case 20: return "Aamir Khan";
                case 21: return "Nicole Kidman";
                default: return "Thiên Ân Femin (tiktoker)";
            }
        }
        
    }
}
