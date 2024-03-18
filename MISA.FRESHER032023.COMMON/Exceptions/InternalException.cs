
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MISA.FRESHER032023.COMMON.Exceptions
{
    public class InternalException : Exception
    {
        public InternalException() { }

        public InternalException(string message) : base(message) { }
    }
}
