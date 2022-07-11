using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Term7MovieCore.Data.Request
{
    public class TheaterFilterRequest : ParentFilterRequest
    {
        public bool IsIncludeRoom { set; get;} = true;
    }
}
