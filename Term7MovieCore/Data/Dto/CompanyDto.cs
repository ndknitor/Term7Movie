using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Term7MovieCore.Data.Dto
{
    public class CompanyDto
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string LogoUrl { set; get; }
        public bool IsActive { set; get; }
    }
}
