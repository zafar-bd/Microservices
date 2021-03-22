using System;

namespace Microservices.Common.Messages
{
    public class GlobalExceptionMessage
    {
        public string UserName { get; set; }
        public DateTimeOffset OccurredAt { get; set; }
        public string ApplicationName { get; set; }
        public string StackTrace { get; set; }
        public string ExceptionMessage { get; set; }
        public string InnerExceptionMessage { get; set; }
        public string FunctionName { get; set; }
    }
}
