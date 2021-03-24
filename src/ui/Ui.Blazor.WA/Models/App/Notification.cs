using System;

namespace Ui.Blazor.WA.Models.App
{
    public class Notification
    {
        public Guid UserId { get; set; }
        public string MessageFor { get; set; }
        public string Message { get; set; }
        public string MessageFrom { get; set; }
        public DateTimeOffset? MessageSent { get; set; }
    }
}
