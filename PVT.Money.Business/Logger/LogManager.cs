using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PVT.Money.Business.Extensions;
using PVT.Money.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVT.Money.Business.Logger
{
    public class LogManager : ILogManager
    {
        protected DatabaseContext DBContext { get; set; }
        private readonly UserManager<ApplicationUser> _userManager;

        internal LogManager(DatabaseContext dbContext, UserManager<ApplicationUser> userManager)
        {
            DBContext = dbContext;
            _userManager = userManager;
        }

        public async Task WriteAsync(string userName, string logEvent)
        {
            ApplicationUser user = await _userManager.GetUserAsync(userName);

            DateTime date = DateTime.Now;
            await DBContext.LogEvents.AddAsync(new LogEventEntity() { Date = date, UserID = user.Id, Event = logEvent });
            await DBContext.SaveChangesAsync();
        }

        ///////////////////////////////////////
        /// ANTIPATTERN: SPAGETTI
        ///////////////////////////////////////
        public async Task WriteRoleChangingAsync(string userName, string roleName, List<string> newPerms, List<string> removedPerms, List<string> addedPerms)
        {
            if (removedPerms.Count == 0 && addedPerms.Count == 0)
                return;

            string logEvent;


            if (await _userManager.FindByNameAsync(userName) == null)
                throw new InvalidOperationException($"There is no '{userName}' user in database");

            if (newPerms.Count == 0)
            {
                logEvent = $"All permissions from role '{roleName}' were removed.";
            }
            else
            {
                logEvent = $"Role '{roleName}' permissions were changed to";

                foreach (string perm in newPerms)
                {
                    logEvent += $" '{perm}',";
                }
                logEvent = logEvent.TrimEnd(',');
                logEvent += ".";
            }

            if (removedPerms.Count != 0)
            {
                logEvent += removedPerms.Count == 1 ? " Permission" : " Permissions";
                foreach (string perm in removedPerms)
                {
                    logEvent += $" '{perm}',";
                }
                logEvent = logEvent.TrimEnd(',');
                logEvent += removedPerms.Count == 1 ? " was removed." : " were removed.";
            }

            if (addedPerms.Count != 0)
            {
                logEvent += addedPerms.Count == 1 ? " Permission" : " Permissions";
                foreach (string perm in addedPerms)
                {
                    logEvent += $" '{perm}',";
                }
                logEvent = logEvent.TrimEnd(',');
                logEvent += addedPerms.Count == 1 ? " was added." : " were added.";
            }

            await WriteAsync(userName, logEvent);
        }

        public async Task<IEnumerable<LogEvent>> GetLogEventsAsync()
        {
            List<LogEvent> result = new List<LogEvent>();

            IEnumerable<LogEventEntity> logEntities = await DBContext.LogEvents.Include(l => l.User).ToListAsync();

            foreach (LogEventEntity entity in logEntities)
            {
                result.Add(new LogEvent() { UserName = entity.User.UserName, Date = entity.Date, Message = entity.Event });
            }

            result.Sort((e1, e2) => e1.Date.CompareTo(e2.Date));
            return result;
        }

        public async Task<IEnumerable<LogEvent>> GetLogEventsAsync(DateTime dateFrom, DateTime dateTo, string username = null)
        {
            List<LogEvent> result = new List<LogEvent>();

            dateTo = dateTo.Date;
            dateTo = dateTo.AddDays(1);

            IEnumerable<LogEventEntity> log = await DBContext.LogEvents.Include(l => l.User).Where(l => l.Date >= dateFrom && l.Date < dateTo).ToArrayAsync();

            foreach (LogEventEntity entity in log)
            {
                if (username == null)
                    result.Add(new LogEvent() { UserName = entity.User.UserName, Date = entity.Date, Message = entity.Event });
                else if (username == entity.User.UserName)
                    result.Add(new LogEvent() { UserName = entity.User.UserName, Date = entity.Date, Message = entity.Event });
            }

            result.Sort((e1, e2) => e1.Date.CompareTo(e2.Date));
            return result;
        }
    }
}
