using System;

namespace Microservices.Exceptions.Data.Domains
{
    public class GlobalExceptionMessage
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public DateTimeOffset OccurredAt { get; set; }
        public string ApplicationName { get; set; }
        public string StackTrace { get; set; }
        public string ExceptionMessage { get; set; }
        public string InnerExceptionMessage { get; set; }
        public string FunctionName { get; set; }
    }
}
