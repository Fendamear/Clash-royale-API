using ClashRoyaleApi.Data;
using ClashRoyaleApi.DTOs.River_Race_Season_Log;
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
        [TestInitialize]
        public void Initialize()
        {
            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetSeasonID_ShouldReturnWithArgumentException()
        {
            //arrange
            var SectionId = 1;
            var type = PeriodType.COLOSSEUM;


            var riverRaceLogs = new List<DbRiverRaceLog>();
            var expectedRecord = new DbRiverRaceLog();
            riverRaceLogs.Add(expectedRecord);

            var mockContext = new Mock<DataContext>();
            mockContext.Setup(dc => dc.RiverRaceLogs).Returns(MockDbSet(riverRaceLogs));

            var riverRace = new CurrentRiverRaceLogic(mockContext.Object);

            //act
            int result = riverRace.GetSeasonId(SectionId, PeriodType.WARDAY);
        }

        [TestMethod]
        public void GetSeasonID_ShouldReturnSameSeasonID()
        {
            var SectionId = 1;
            var type = PeriodType.COLOSSEUM;

            TestHelperClass helper = new TestHelperClass();

            var riverRaceLogs = new List<DbRiverRaceLog>();
            var expectedRecord = helper.GetCurrentRiverRaceLog(SectionId, type);
            riverRaceLogs.Add(expectedRecord);

            var mockContext = new Mock<DataContext>();

            mockContext.Setup(dc => dc.RiverRaceLogs).Returns(MockDbSet(riverRaceLogs));

            var riverRace = new CurrentRiverRaceLogic(mockContext.Object);

            //act
            int result = riverRace.GetSeasonId(SectionId, PeriodType.WARDAY);

            //assert
            Assert.AreEqual(100, result);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetSeasonID_ShouldReturnWithException()
        {
            var oldSectionId = 1;
            var newSectionId = 2;
            var seasonId = 100;
            var type = PeriodType.TRAINING;

            TestHelperClass helper = new TestHelperClass();

            var riverRaceLogs = new List<DbRiverRaceLog>();
            var expectedRecord = helper.GetCurrentRiverRaceLog(oldSectionId, type);
            riverRaceLogs.Add(expectedRecord);

            var mockContext = new Mock<DataContext>();

            mockContext.Setup(dc => dc.RiverRaceLogs).Returns(MockDbSet(riverRaceLogs));

            var riverRace = new CurrentRiverRaceLogic(mockContext.Object);

            //act
            int result = riverRace.GetSeasonId(newSectionId, type);

            //arrange
            Assert.AreEqual(result, seasonId);
            mockContext.Verify(x => x.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void GetSeasonID_ShouldReturnCurrentSeasonIDwithSectionIncrement()
        {
            //arrange
            var oldSectionId = 1;
            var type = PeriodType.WARDAY;

            TestHelperClass helper = new TestHelperClass();

            var riverRaceLogs = new List<DbRiverRaceLog>();
            var expectedRecord = helper.GetCurrentRiverRaceLog(oldSectionId, type);
            riverRaceLogs.Add(expectedRecord);

            var mockContext = new Mock<DataContext>();

            mockContext.Setup(dc => dc.RiverRaceLogs).Returns(MockDbSet(riverRaceLogs));

            var riverRace = new CurrentRiverRaceLogic(mockContext.Object);

            //act
            int result = riverRace.GetSeasonId(2, PeriodType.WARDAY);

            //arrange
            Assert.AreEqual(result, 100);
            mockContext.Verify(x => x.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void GetSeasonID_ShouldReturnSeasonIDWithIncrement()
        {

            var oldSectionId = 3;
            var newSectionId = 0;
            var seasonId = 101;
            var type = PeriodType.COLOSSEUM;

            TestHelperClass helper = new TestHelperClass();

            var riverRaceLogs = new List<DbRiverRaceLog>();
            var expectedRecord = helper.GetCurrentRiverRaceLog(oldSectionId, type);
            riverRaceLogs.Add(expectedRecord);

            var mockContext = new Mock<DataContext>();

            mockContext.Setup(dc => dc.RiverRaceLogs).Returns(MockDbSet(riverRaceLogs));

            var riverRace = new CurrentRiverRaceLogic(mockContext.Object);

            //act
            int result = riverRace.GetSeasonId(newSectionId, PeriodType.WARDAY);

            //arrange
            Assert.AreEqual(result, seasonId);
            mockContext.Verify(x => x.SaveChanges(), Times.Once());
        }

        public DbSet<T> MockDbSet<T>(List<T> data) where T : class
        {
            var queryable = data.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            return mockSet.Object;
        }
    }
}
