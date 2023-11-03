using ClashRoyaleApi.Data;
using ClashRoyaleApi.Logic.CurrentRiverRace;
using ClashRoyaleApi.Models.CurrentRiverRace;
using ClashRoyaleApi.Models.CurrentRiverRace.CRR_Response;
using ClashRoyaleApi.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
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
    public class UpdateExistingRiverRaceTest
    {

        [TestInitialize]
        public void Initialize()
        {

        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateExistingRiverRaceData_ShouldReturnWithArgumentException()
        {
            var currentRiverRace = new List<DbCurrentRiverRace>();
            var expectedRecord = new DbCurrentRiverRace();
            currentRiverRace.Add(expectedRecord);

            var mockContext = new Mock<DataContext>();
            mockContext.Setup(dc => dc.CurrentRiverRace).Returns(MockDbSet(currentRiverRace));

            var riverRace = new CurrentRiverRaceLogic(mockContext.Object);

            //act
            List<NrOfAttacksRemaining> nrOfAttacksRemainings = new List<NrOfAttacksRemaining>();
            CurrentRiverRaceLogic logic = new CurrentRiverRaceLogic(mockContext.Object);

            nrOfAttacksRemainings = logic.UpdateExistingRiverRaceData(new Root() { sectionIndex = 2 }, 100, 0, SchedulerTime.MINUTESBEFORE5);
        }

        [TestMethod]
        public void UpdateExistingRiverRaceData_TimerIsBeforelastRecordedEntry_ShouldReturnEmptyList()
        {
            //arrange

            SchedulerTime time = SchedulerTime.MINUTESBEFORE120;

            TestHelperClass helper = new TestHelperClass();
            var currentRiverRace = new List<DbCurrentRiverRace>();

            var expectedRecord = helper.GetCurrentRiverRace();
            currentRiverRace.Add(expectedRecord);

            var mockContext = new Mock<DataContext>();
            mockContext.Setup(dc => dc.CurrentRiverRace).Returns(MockDbSet(currentRiverRace));

            var riverRace = new CurrentRiverRaceLogic(mockContext.Object);

            //act
            List<NrOfAttacksRemaining> nrOfAttacksRemainings = new List<NrOfAttacksRemaining>();
            CurrentRiverRaceLogic logic = new CurrentRiverRaceLogic(mockContext.Object);

            nrOfAttacksRemainings = logic.UpdateExistingRiverRaceData(new Root() { sectionIndex = 2 }, 100, 0, time);

            Assert.AreEqual(0, nrOfAttacksRemainings.Count);
        }

        [TestMethod]
        public void UpdateExistingRiverRaceData_TimerIsBeforelastRecordedEntry_ShouldReturnListWithClanMembers()
        {
            //arrange

            SchedulerTime time = SchedulerTime.MINUTESBEFORE5;

            TestHelperClass helper = new TestHelperClass();
            var currentRiverRace = new List<DbCurrentRiverRace>();

            var expectedRecord = helper.GetCurrentRiverRaceList();
            currentRiverRace.AddRange(expectedRecord);

            var mockContext = new Mock<DataContext>();
            mockContext.Setup(dc => dc.CurrentRiverRace.Where(x => x.SeasonId == 100)).Returns(MockDbSet(currentRiverRace));  
            
            var riverRace = new CurrentRiverRaceLogic(mockContext.Object);


            //act
            List<NrOfAttacksRemaining> nrOfAttacksRemainings = new List<NrOfAttacksRemaining>();
            CurrentRiverRaceLogic logic = new CurrentRiverRaceLogic(mockContext.Object);

            nrOfAttacksRemainings = logic.UpdateExistingRiverRaceData(new Root() { sectionIndex = 2 }, 100, 0, time);

            Assert.AreEqual(5, nrOfAttacksRemainings.Count);
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
