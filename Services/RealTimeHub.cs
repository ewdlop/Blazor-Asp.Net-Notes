using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp.Services
{
    public class RealTimeHub : Hub
    {
        public async Task UserConnected()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "All");
        }

        public async Task SendMessage(string user, string message) 
        {
            await Clients.Group("All").SendAsync("ReceiveMessage", user, message);
        }

        public override async Task OnDisconnectedAsync(Exception exception) 
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "All");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
