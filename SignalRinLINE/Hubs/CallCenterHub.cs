using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRinLINE.Models;
using SignalRinLINE.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRinLINE.Hubs
{
    [Authorize]
    public class CallCenterHub : Hub
    {
        private readonly IGroupService _groupService;
        private readonly IHubContext<ChatHub> _chathub;
        public CallCenterHub(IGroupService groupService, IHubContext<ChatHub> chathub)
        {
            _groupService = groupService;
            _chathub = chathub;
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("ActiveGroups", await _groupService.GetAllGroups());
            await base.OnConnectedAsync();
        }

        public async Task SendCallCenterMessage(string groupId, string text)
        {
            var message = new ChatMessage
            {
                LineName = Context.User.Identity.Name,
                Text = text,
                SendTime = DateTime.Now
            };

            await _groupService.AddMessage(groupId, message);

            await _chathub.Clients.Group(groupId.ToString()).SendAsync("ReceiveMessage",
                    message.LineName,
                    message.LineID,
                    message.LinePic,
                    message.SendTime,
                    message.Text);
        }

        public async Task LoadHistory(string groupId)
        {
            var history = await _groupService.GetMessageHistory(groupId);
 
            await Clients.Caller.SendAsync("ReceiveMessages", history);
        }

    }
}
