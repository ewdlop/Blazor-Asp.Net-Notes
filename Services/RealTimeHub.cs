using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace BlazorServerApp.Services
{
    public class RealTimeHub : Hub
    {
        public static ConcurrentDictionary<string, List<string>> ConnectedUsers = new ConcurrentDictionary<string, List<string>>();

        public async Task UserConnectedAsync()
        {
            //ConnectedUsers.TryGetValue(Context.User.FindFirstValue(ClaimTypes.NameIdentifier), out List<string> existingUserConnectionIds);
            //(existingUserConnectionIds ??= new List<string>()).Add(Context.ConnectionId);
            ////ConnectedUsers.TryAdd(Context.User.FindFirstValue(ClaimTypes.NameIdentifier), existingUserConnectionIds);
            ////Context.User.Claims.ToList().ForEach(async claim => await Groups.AddToGroupAsync(Context.ConnectionId, claim.Value.ToLower()));
            await Groups.AddToGroupAsync(Context.ConnectionId, "All");
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.Group("All").SendAsync("ReceiveMessage", user, message);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //ConnectedUsers.TryGetValue(Context.User.FindFirstValue(ClaimTypes.NameIdentifier), out List<string> existingUserConnectionIds);
            //existingUserConnectionIds.Remove(Context.ConnectionId);
            //if (existingUserConnectionIds.Count == 0)
            //{
            //    ConnectedUsers.TryRemove(Context.User.FindFirstValue(ClaimTypes.NameIdentifier), out List<string> garbage);
            //}
            //Context.User.Claims.ToList().ForEach(async claim => await Groups.RemoveFromGroupAsync(Context.ConnectionId, claim.Value.ToLower()));
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "All");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
