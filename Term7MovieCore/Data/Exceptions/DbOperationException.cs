using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Term7MovieCore.Data.Exceptions
{
    public class DbOperationException : Exception
    {
        public DbOperationException() : base()
        {

        }

        public DbOperationException(string message) : base(message)
        {

        }
    }
}
