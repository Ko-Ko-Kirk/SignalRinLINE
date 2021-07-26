using SignalRinLINE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRinLINE.Services
{
    public interface IGroupService
    {
        Task<string> CreateGroup(string connectionId, string lineID);

        Task<string> GetGroupForConnectionId(string connectionId);

        Task SetGroupName(string GroupId, string name);

        Task AddMessage(string GroupId, ChatMessage message);

        Task<IEnumerable<ChatMessage>> GetMessageHistory(string groupId);

        Task<IReadOnlyDictionary<string, ChatGroup>> GetAllGroups();

        Task<Boolean> CheckGroupIfExist(string lineID);
    }
}
