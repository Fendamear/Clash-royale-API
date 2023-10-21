using ClashRoyaleApi.Data;
using ClashRoyaleApi.Logic.ClanMembers;
using ClashRoyaleApi.Logic.CurrentRiverRace;
using ClashRoyaleApi.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.Logic_Layer_Test.TestHelper;
using static ClashRoyaleApi.Models.EnumClass;

namespace UnitTest.Logic_Layer_Test.CurrentRiverRace
{

    [TestClass()]
    public class GetSeasonIDTest
    {

        private Mock<DataContext> _dataContextMock;
        private CurrentRiverRaceLogic _logic;
        private TestHelperClass _helperClass;
        private Mock<DbSet<DbRiverRaceLog>> _dbSetMock;

        [TestInitialize]
        public void Initialize()
        {
            _dataContextMock = new Mock<DataContext>();
            _helperClass = new TestHelperClass();
            _dbSetMock = new Mock<DbSet<DbRiverRaceLog>>();
        }

        [TestMethod]
        public async Task Test1()
        {
            //arrange
            var sectionId = 1;
            var type = PeriodType.COLLOSEUM;

            // Set up the mock data context to return a null lastRecord (indicating no existing records).
            _dataContextMock.Setup(x => x.RiverRaceLogs).Returns(_helperClass.GetCurrentRiverRaceLog(sectionId, type);
            var currentRiverRace = new CurrentRiverRaceLogic(_dataContextMock.Object);

            // Act
            int result = currentRiverRace.GetSeasonId(0, PeriodType.WARDAY);

            // Assert
            // Add assertions for the result and verify the SaveChanges method on the mock data context.

            //act
            //assert
        }

    }
}
