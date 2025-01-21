using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace API.SignalR;

[Authorize]
public class PresenceHub(PresenceTracker tracker) : Hub
{
    public override async Task OnConnectedAsync()
    {
        if (Context.User == null) throw new HubException("Cannot get current usernamr");
        await tracker.UserConnected(Context.User.GetUsername(), Context.ConnectionId);
        await Clients.Others.SendAsync("UserIsInOnline", Context.User?.GetUsername());

        var currentUser = await tracker.GetOnlineUsers();
        await Clients.All.SendAsync("GetOnlineUsers", currentUser);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (Context.User == null) throw new HubException("Cannot get current usernamr");

        await tracker.UserDisconnected(Context.User.GetUsername(), Context.ConnectionId);
        await Clients.Others.SendAsync("UserIsOffOnline", Context.User?.GetUsername());
        
        var currentUser = await tracker.GetOnlineUsers();
        await Clients.All.SendAsync("GetOnlineUsers", currentUser);
        
        await base.OnDisconnectedAsync(exception);
    }

}
