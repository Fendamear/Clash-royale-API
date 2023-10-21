using ClashRoyaleApi.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Logic_Layer_Test.TestHelper
{
    public class TestHelper
    {
        TestHelper() 
        { 
        
        }

        public DbCurrentRiverRace GetOneCurrentRiverRaceForGetSeasonId(int seasonId, int sectionId)
        {
            return new DbCurrentRiverRace(Guid.Parse("db479f3a-91cd-4b6b-8d7b-9c86574c869e"), seasonId, sectionId, 0, "100.0.0", "Tag", "Name", 900, 4, 0, ClashRoyaleApi.Models.EnumClass.SchedulerTime.Schedule0800);
        }
    }
}
