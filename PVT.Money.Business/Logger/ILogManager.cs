using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PVT.Money.Business.Logger
{
    public interface ILogManager
    {
        Task WriteAsync(string userName, string logEvent);
        Task WriteRoleChangingAsync(string userName, string roleName, List<string> newPerms, List<string> removedPerms, List<string> addedPerms);
        Task<IEnumerable<LogEvent>> GetLogEventsAsync();
        Task<IEnumerable<LogEvent>> GetLogEventsAsync(DateTime dateFrom, DateTime dateTo, string username = null);
    }
}
