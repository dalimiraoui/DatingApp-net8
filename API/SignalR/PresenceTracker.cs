using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR;

[Authorize]
public class PresenceTracker
{
    private static readonly  Dictionary<string, List<string>> OnlineUsers = [];

    public Task<bool> UserConnected(string username, string connectedId)
    {
        var isOnline = false;
        lock (OnlineUsers)
        {
            if (OnlineUsers.ContainsKey(username)) 
            {
                OnlineUsers[username].Add(connectedId);
            }
            else 
            {
                OnlineUsers.Add(username, [connectedId]);
                isOnline =true;
            }
            return Task.FromResult(isOnline);
        }

    }

    public Task<bool> UserDisconnected(string username, string connectedId)
    {
        var isOffline = false;
        lock (OnlineUsers)
        {
            if (!OnlineUsers.ContainsKey(username)) 
            {
                return Task.FromResult(isOffline);
            }
            else 
            {
                OnlineUsers[username].Remove(connectedId);

                if (OnlineUsers[username].Count == 0) {
                    OnlineUsers.Remove(username);
                    isOffline =true;
                }
            }
        }
        return Task.FromResult(isOffline);

    }

    public Task<string[]> GetOnlineUsers()
    {
        string[] onlineUsers;
        lock (OnlineUsers)
        {
        onlineUsers = OnlineUsers.OrderBy(k => k.Key).Select(k => k.Key).ToArray();
        }
        return Task.FromResult(onlineUsers);
    }

    public static Task<List<string>> GetConnectionsForUser(string  username)
    {
        List<string> connectionIds;
        if (OnlineUsers.TryGetValue(username, out var connections))
        {
            lock(connections)
            {
                connectionIds = connections.ToList();
                // since 12 c# version we can write the instruction as below
                //connectionIds = [.. connections];
            }

        }
        else
        {
            connectionIds = [];
        }

        return Task.FromResult(connectionIds);
    }

}
