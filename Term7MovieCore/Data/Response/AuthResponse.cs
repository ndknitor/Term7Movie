using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Term7MovieCore.Data.Response
{
    public class AuthResponse : ParentResponse
    {
        public string AccessToken { set; get; }
        public string RefreshToken { set; get; }
    }
}
