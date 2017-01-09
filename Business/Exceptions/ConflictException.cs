using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace loconotes.Business.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException() { }

        public ConflictException(string message) : base(message) { }

        public ConflictException(string message, params object[] args) : base(string.Format(message, args)) { }

        public ConflictException(string message, Exception inner) : base(message, inner) { }

    }
}
