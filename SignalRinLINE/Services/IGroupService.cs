using SignalRinLINE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRinLINE.Services
{
    public interface IGroupService
    {
        Task<Guid> CreateGroup(string connectionId);

        Task<Guid> GetGroupForConnectionId(string connectionId);

        Task SetGroupName(Guid GroupId, string name);

        Task AddMessage(Guid GroupId, ChatMessage message);

        Task<IEnumerable<ChatMessage>> GetMessageHistory(Guid roomId);

        Task<IReadOnlyDictionary<Guid, ChatGroup>> GetAllGroups();
    }
}
