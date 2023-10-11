using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Shared.Exceptions
{
    public class SalesException : Exception
    {
        public SalesException()
        {
        }

        public SalesException(string message) : base(message)
        {
        }

        public SalesException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
