using SignalRinLINE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRinLINE.Services
{
    public class GroupService : IGroupService
    {
        public Task AddMessage(Guid GroupId, ChatMessage message)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> CreateGroup(string connectionId)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyDictionary<Guid, ChatGroup>> GetAllGroups()
        {
            throw new NotImplementedException();
        }

        public Task<Guid> GetGroupForConnectionId(string connectionId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ChatMessage>> GetMessageHistory(Guid roomId)
        {
            throw new NotImplementedException();
        }

        public Task SetGroupName(Guid GroupId, string name)
        {
            throw new NotImplementedException();
        }
    }
}
