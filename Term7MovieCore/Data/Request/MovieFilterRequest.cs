using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Term7MovieCore.Data.Request
{
    public class MovieFilterRequest : ParentFilterRequest
    {
        public bool IsAvailableOnly { get; set; } = false;
        public bool IsDisabledOnly { set; get; } = false;
    }
}
