using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Term7MovieCore.Data.Request
{
    public class TransactionFilterRequest : ParentFilterRequest
    {
        public Guid? TransactionId { set; get; }
        public int? StatusId { set; get; }
    }
}
