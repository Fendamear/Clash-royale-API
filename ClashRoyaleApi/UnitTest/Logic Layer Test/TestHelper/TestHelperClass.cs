using ClashRoyaleApi.Models.CurrentRiverRace;
using ClashRoyaleApi.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static ClashRoyaleApi.Models.EnumClass;

namespace UnitTest.Logic_Layer_Test.TestHelper
{
    public class TestHelperClass
    {
        public TestHelperClass() 
        { 
            
        
        }

        public DbRiverRaceLog GetCurrentRiverRaceLog(int sectionId, PeriodType type) 
        {
            return new DbRiverRaceLog(Guid.Parse("db479f3a-91cd-4b6b-8d7b-9c86574c869e"), DateTime.Now, 100, sectionId, type);
        }
        
        public DbCurrentRiverRace GetCurrentRiverRace()
        {
            return new DbCurrentRiverRace(Guid.NewGuid(), 100, 0,0, "100.0.0", "Tag", "Name", 900, 4, 0, SchedulerTime.SCHEDULE1100);
        }

        public List<DbCurrentRiverRace> GetCurrentRiverRaceList()
        {
            List<DbCurrentRiverRace> list = new List<DbCurrentRiverRace>();

            list.Add(GetCurrentRiverRace());
            list.Add(GetCurrentRiverRace());
            list.Add(GetCurrentRiverRace());
            list.Add(GetCurrentRiverRace());
            list.Add(GetCurrentRiverRace());
            return list;
        }

        public List<DbClanMembers> GetClanMembers(string role, DateTime dateTime, bool isActive, bool isInClan)
        {
            List<DbClanMembers> dbClanMembers = new List<DbClanMembers>
            {
                new DbClanMembers("#L2880298", "zuenhairygranny", "elder", DateTime.Now, true, true),
                new DbClanMembers("#2QPP2JLP8", "Piet", "coLeader", DateTime.Now, true, true),
                new DbClanMembers("#Y2VPQL0C", "niekuden", "elder", DateTime.Now, true, true),
                new DbClanMembers("#9JL98CQUY", "Kingkiller", "elder", DateTime.Now, true, true),
                new DbClanMembers("#VUUYJ9YC", "britt power", role, dateTime, isActive, isInClan)
            };
            return dbClanMembers;
        }

        public List<DbClanMembers> GetClanMember(string role, DateTime dateTime, bool isActive, bool isInClan)
        {
            List<DbClanMembers> dbClanMembers = new List<DbClanMembers>
            {
                new DbClanMembers("#L2880298", "zuenhairygranny", role, dateTime, isActive, isInClan)
            };
            return dbClanMembers;
        }
    }
}
