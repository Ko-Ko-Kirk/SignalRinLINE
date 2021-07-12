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

        public async Task SendMessage(string name, string id, string pic, string text)
        {
            var groupId = await _groupService.GetGroupForConnectionId(Context.ConnectionId);

            var message = new ChatMessage
            {
                LineName = name,
                LineID = id,
                LinePic = pic,
                Text = text,
                SendTime = DateTime.Now
            };

            await _groupService.AddMessage(groupId, message);

            await Clients.Group(groupId.ToString()).SendAsync("ReceiveMessage",
                message.LineName,
                message.LineID,
                message.LinePic,
                message.SendTime,
                message.Text);
        }

        public async Task SetName(string lineName, string lineID)
        {
            var groupName = $"{lineName} {lineID}";

            var groupId = await _groupService.GetGroupForConnectionId(Context.ConnectionId);

            await _groupService.SetGroupName(groupId, groupName);

            await _callCenterHub.Clients.All.SendAsync("ActiveRooms", await _groupService.GetAllGroups());
        }

        public override async Task OnConnectedAsync()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                await base.OnConnectedAsync();
                return;
            }

            var groupId = await _groupService.CreateGroup(Context.ConnectionId);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupId.ToString());

            await Clients.Caller.SendAsync("ReceiveMessage","KOKO的服務中心",
                DateTime.Now,
                "您好，請問我可以幫您什麼忙嗎？");

            await base.OnConnectedAsync();
        }


        [Authorize]
        public async Task JoinGroup(Guid groupId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId.ToString());
        }

        [Authorize]
        public async Task LeaveGroup(Guid groupId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId.ToString());
        }

    }
}
