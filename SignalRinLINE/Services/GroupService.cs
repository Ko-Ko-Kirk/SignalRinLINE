using SignalRinLINE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRinLINE.Services
{
    public class GroupService : IGroupService
    {

        private readonly Dictionary<Guid, ChatGroup> _groupInfo = 
            new Dictionary<Guid, ChatGroup>();

        private readonly Dictionary<Guid, List<ChatMessage>> _messageHistory = 
            new Dictionary<Guid, List<ChatMessage>>();

        public Task AddMessage(Guid GroupId, ChatMessage message)
        {
            if (!_messageHistory.ContainsKey(GroupId))
            {
                _messageHistory[GroupId] = new List<ChatMessage>();
            }

            _messageHistory[GroupId].Add(message);

            return Task.CompletedTask;
        }

        public Task<Guid> CreateGroup(string connectionId)
        {
            var id = Guid.NewGuid();
            _groupInfo[id] = new ChatGroup
            {
                GroupConnectionId = connectionId, 
            };

            return Task.FromResult(id);
        }

        public Task<IReadOnlyDictionary<Guid, ChatGroup>> GetAllGroups()
        {
            return Task.FromResult(_groupInfo as IReadOnlyDictionary<Guid, ChatGroup>);
        }

        public Task<Guid> GetGroupForConnectionId(string connectionId)
        {
            return Task.FromResult(_groupInfo.FirstOrDefault(
                            x => x.Value.GroupConnectionId == connectionId).Key);
        }

        public Task<IEnumerable<ChatMessage>> GetMessageHistory(Guid groupId)
        {
            _messageHistory.TryGetValue(groupId, out var messages);

            messages = messages ?? new List<ChatMessage>();

            return Task.FromResult(messages.OrderBy(x => x.SendTime).AsEnumerable());
        }

        public Task SetGroupName(Guid groupId, string name)
        {
            if (!_groupInfo.ContainsKey(groupId))
                throw new ArgumentException("Invalid group ID");

            _groupInfo[groupId].GroupName = name;

            return Task.CompletedTask;
        }
    }
}
