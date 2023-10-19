using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashRoyaleApi.Models
{
    public class EnumClass
    {
        public enum MemberStatus
        {
            Removed,
            Joined,
            RoleChange
        }

        public enum PeriodType
        {
            TRAINING,
            WARDAY,
            COLLOSEUM
        }

        public enum SchedulerTime
        {
            Schedule0800,
            SCHEDULE1030,
            SCHEDULE1100,
            SCHEDULE1130,
        }

        public enum Status
        {
            SUCCES,
            FAILED
        }
    }
}
