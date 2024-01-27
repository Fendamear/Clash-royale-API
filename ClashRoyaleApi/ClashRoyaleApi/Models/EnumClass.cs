using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
            [EnumMember(Value = "Training day")]
            TRAINING,

            [EnumMember(Value = "War day")]
            WARDAY, 
            
            [EnumMember(Value = "Colosseum")]
            COLOSSEUM
        }

        public enum SchedulerTime
        {
            [EnumMember(Value = "3 hours before deadline")]
            MINUTESBEFORE180,
            [EnumMember(Value = "2 hours before deadline")]
            MINUTESBEFORE120,
            [EnumMember(Value = "1 hour before deadline")]
            MINUTESBEFORE60,
            [EnumMember(Value = "30 minutes before deadline")]
            MINUTESBEFORE30,
            [EnumMember(Value = "5 minutes before deadline")]
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

        public enum JWTToken
        {
            ID,
            ClanTag
        }
    }
}
