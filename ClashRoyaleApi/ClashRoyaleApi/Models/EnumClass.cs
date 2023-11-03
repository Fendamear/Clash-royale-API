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
            COLOSSEUM
        }

        public enum SchedulerTime
        {
            MINUTESBEFORE180,
            MINUTESBEFORE120,
            MINUTESBEFORE60,
            MINUTESBEFORE30,
            MINUTESBEFORE5,
            CLANINFOSCHEDULE
        }

        public enum Status
        {
            SUCCES,
            FAILED
        }

        public enum MailType
        {
            BUILD,
            ATTACKSREMAINING
        }

        public enum RoyaleApiType
        {
            CURRENTRIVERRACE,
            CLANMEMBERINFO,
            RIVERRACE
        }
    }
}
