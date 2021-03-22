using System;

namespace Microservices.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(string.Format(ApplicationExceptionMessage.NotFound, message))
        {

        }
    }
    public class QueueEmptyException : Exception
    {
        public QueueEmptyException(string message) : base(message)
        {

        }
    }
}
