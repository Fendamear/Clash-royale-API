using ClashRoyaleApi.Logic.Logging.LoggingModels;
using ClashRoyaleApi.Models.DbModels;
using System;
using System.Collections.Generic;
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
        



    }
}
