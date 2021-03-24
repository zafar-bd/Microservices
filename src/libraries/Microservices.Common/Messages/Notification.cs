using System;
namespace Microservices.Common.Messages
{
   public class Notification
    {
        public bool IsError { get; set; }
        public Guid UserId { get; set; }
        public string MessageFor { get; set; }
        public string Message { get; set; }
        public string MessageFrom { get; set; }
        public DateTimeOffset? MessageSent { get; set; }
    }
}
