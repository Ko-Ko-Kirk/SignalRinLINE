using Microsoft.AspNetCore.SignalR;
using SignalRinLINE.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRinLINE.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IGroupService _groupService;



        public async Task SendMessage(string user, string message) 
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public override async Task OnConnectedAsync()
        {
            var username = Context.User.Identity.Name;

            await Groups.AddToGroupAsync(Context.ConnectionId, "??");
            await Clients.Caller.SendAsync("ReceiveMessage","KoKo", "");
            await base.OnConnectedAsync();
        }
        public string GetConnectionId() => Context.ConnectionId;

    }
}
