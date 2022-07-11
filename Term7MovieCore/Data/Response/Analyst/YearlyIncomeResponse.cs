using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Term7MovieCore.Data.Dto.Analyst;

namespace Term7MovieCore.Data.Response.Analyst
{
    public class YearlyIncomeResponse : ParentResponse
    {
        public IEnumerable<YearlyIncomeDTO> Result { get; set; }
        public decimal LowestIncome { get; set; }
        public decimal HighestIncome { get; set; }
    }
}
