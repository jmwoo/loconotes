using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace loconotes.Business.Exceptions
{
    public class NotFoundException : System.Exception
    {
	    public NotFoundException()
	    {
	    }

        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string message, params object[] args) : base(string.Format(message, args)) { }

        public NotFoundException(string message, Exception inner) : base(message, inner) { }

    }
}
