using SignalRinLINE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRinLINE.Services
{
    public class GroupService : IGroupService
    {

        private readonly Dictionary<string, ChatGroup> _groupInfo = 
            new Dictionary<string, ChatGroup>();

        private readonly Dictionary<string, List<ChatMessage>> _messageHistory = 
            new Dictionary<string, List<ChatMessage>>();
        
        public Task AddMessage(string GroupId, ChatMessage message)
        {
            if (!_messageHistory.ContainsKey(GroupId))
            {
                _messageHistory[GroupId] = new List<ChatMessage>();
            }

            _messageHistory[GroupId].Add(message);

            return Task.CompletedTask;
        }

        public Task<string> CreateGroup(string connectionId, string lineID)
        {  
            _groupInfo[lineID] = new ChatGroup
            {
                GroupConnectionId = connectionId, 
            };

            return Task.FromResult(lineID);
        }

        public Task<IReadOnlyDictionary<string, ChatGroup>> GetAllGroups()
        {
            return Task.FromResult(_groupInfo as IReadOnlyDictionary<string, ChatGroup>);
        }

        public Task<Boolean> CheckGroupIfExist(string lineID)
        {
            return Task.FromResult(_groupInfo.ContainsKey(lineID));
        }

        public Task<string> GetGroupForConnectionId(string connectionId)
        {
            return Task.FromResult(_groupInfo.FirstOrDefault(
                            x => x.Value.GroupConnectionId == connectionId).Key);
        }

        public Task<IEnumerable<ChatMessage>> GetMessageHistory(string groupId)
        {
            _messageHistory.TryGetValue(groupId, out var messages);

            messages = messages ?? new List<ChatMessage>();
            var messageOrder = messages.OrderBy(x => x.SendTime).AsEnumerable();
            return Task.FromResult(messageOrder);
        }

        public Task SetGroupName(string groupId, string name)
        {
            if (!_groupInfo.ContainsKey(groupId))
                throw new ArgumentException("Invalid group ID");

            _groupInfo[groupId].GroupName = name;

            return Task.CompletedTask;
        }
    }
}
