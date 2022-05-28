using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Term7MovieCore.Data.Request
{
    public class ExchangeTokenRequest
    {
        [Required]
        public string RefreshToken { set; get; }
    }
}
