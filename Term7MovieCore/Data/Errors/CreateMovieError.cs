using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Term7MovieCore.Data.Dto.Errors
{
    public class CreateMovieError
    {
        //public int MovieId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; } = true;
        //saving for rainy day
        //public IEnumerable<CategoryError> CategoriesStatus { get; set; }
    }

    public class CategoryError
    {
        public int CategoryId { get; set; }
        public string Message { get; set; }
    }
}
