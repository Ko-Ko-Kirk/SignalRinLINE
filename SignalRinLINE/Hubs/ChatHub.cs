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
    public class ChatHub : Hub
    {
        private readonly IGroupService _groupService;
        private readonly IHubContext<CallCenterHub> _callCenterHub;
        public ChatHub(IGroupService groupService, IHubContext<CallCenterHub> callCenterHub)
        {
            _groupService = groupService;
            _callCenterHub = callCenterHub;
        }

        public async Task SendMessage(string name, string lineID, string pic, string text)
        {
            var groupID = await _groupService.GetGroupForConnectionId(Context.ConnectionId);
           
            var message = new ChatMessage
            {
                LineName = name,
                LineID = lineID,
                LinePic = pic,
                Text = text,
                SendTime = DateTime.Now
            };

            await _groupService.AddMessage(lineID, message);

            await Clients.Group(lineID).SendAsync("ReceiveMessage",
                message.LineName,
                message.LineID,
                message.LinePic,
                message.SendTime,
                message.Text);
        }

        public async Task CreateGroupSetName(string lineName, string lineID)
        {
            var groupName = $"{lineName} {lineID}";

            var groupID = await _groupService.CreateGroup(Context.ConnectionId, lineID);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupID.ToString());

            var groupId = await _groupService.GetGroupForConnectionId(Context.ConnectionId);

            await _groupService.SetGroupName(lineID, groupName);

            await _callCenterHub.Clients.All.SendAsync("ActiveGroups", await _groupService.GetAllGroups());
        }


        public override async Task OnConnectedAsync()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                await base.OnConnectedAsync();
                return;
            }

            await Clients.Caller.SendAsync("ReceiveMessage","KOKO的服務中心",
                new ChatMessage().LineID,
                new ChatMessage().LinePic,
                DateTime.Now,
                new ChatMessage().Text);

            await base.OnConnectedAsync();
        }


        [Authorize]
        public async Task JoinGroup(string groupId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
        }

        [Authorize]
        public async Task LeaveGroup(string groupId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);
        }

    }
}
