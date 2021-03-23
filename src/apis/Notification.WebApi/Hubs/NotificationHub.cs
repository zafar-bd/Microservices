using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Notification.WebApi.Hubs
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class NotificationHub : Hub
    {
    }
}
