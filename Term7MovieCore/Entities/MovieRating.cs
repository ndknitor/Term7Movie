using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Term7MovieCore.Entities
{
    public class MovieRating
    {
        public int MovieId { get; set; }
        public long UserId { set; get; }
        public float Rating { set; get; }
        public Movie Movie { set; get; }
        public User User { set; get; }
    }
}
