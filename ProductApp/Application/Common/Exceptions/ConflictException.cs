using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApp.Application.Common.Exceptions
{
    /// <summary>
    /// Represents an error caused by a conflict with the current state of the system
    /// (e.g. duplicate keys, unique constraint violations).
    /// </summary>
    public class ConflictException : Exception
    {
        public ConflictException()
        {
        }

        public ConflictException(string message)
            : base(message)
        {
        }

        public ConflictException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
