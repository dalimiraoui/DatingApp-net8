using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR;

[Authorize]
public class PresenceHub(PresenceTracker tracker) : Hub
{
    public override async Task OnConnectedAsync()
    {
        if (Context.User == null) throw new HubException("Cannot get current usernamr");
        var isOnline = await tracker.UserConnected(Context.User.GetUsername(), Context.ConnectionId);
        if (isOnline) await Clients.Others.SendAsync("UserIsInOnline", Context.User?.GetUsername());

        var currentUser = await tracker.GetOnlineUsers();
        await Clients.All.SendAsync("GetOnlineUsers", currentUser);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (Context.User == null) throw new HubException("Cannot get current usernamr");

        var isOffline = await tracker.UserDisconnected(Context.User.GetUsername(), Context.ConnectionId);
        if (isOffline) await Clients.Others.SendAsync("UserIsOffOnline", Context.User?.GetUsername());

        await base.OnDisconnectedAsync(exception);
    }

}
