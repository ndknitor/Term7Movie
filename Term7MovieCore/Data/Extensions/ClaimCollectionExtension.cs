using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Term7MovieCore.Data.Extensions
{
    public static class ClaimCollectionExtension
    {
        public static string FindFirstValue(this IEnumerable<Claim> list, string type)
        {
            Claim? claim = list.FirstOrDefault(c => c.Type.Equals(type));

            if (claim == null) return null;

            return claim.Value;
        }
    }
}
