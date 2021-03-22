using System;

namespace Microservices.Common.Exceptions
{
    public class DuplicateException : Exception
    {
        public DuplicateException(string message) : base(string.Format(ApplicationExceptionMessage.AlreadyExists, message))
        {

        }
    }
}
