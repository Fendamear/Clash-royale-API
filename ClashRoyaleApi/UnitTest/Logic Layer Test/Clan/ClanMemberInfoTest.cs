using Castle.Core.Configuration;
using ClashRoyaleApi.Data;
using ClashRoyaleApi.Logic.ClanMembers;
using ClashRoyaleApi.Logic.CurrentRiverRace;
using ClashRoyaleApi.Logic.RoyaleApi;
using ClashRoyaleApi.Models.CurrentRiverRace.CRR_Response;
using ClashRoyaleApi.Models.DbModels;
using Microsoft.AspNetCore.Hosting;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClashRoyaleApi.Models.EnumClass;
using UnitTest.Logic_Layer_Test.TestHelper;
using Microsoft.EntityFrameworkCore;
using ClashRoyaleApi.Models.ClanMembers;

namespace UnitTest.Logic_Layer_Test.Clan
{
    [TestClass()]
    public class ClanMemberInfoTest
    {
        private Mock<DataContext> _dataContextMock;
        private Mock<HttpMessageHandler> _handlerMock;
        private ClanMemberLogic _logic;
        

        [TestInitialize]
        public void Initialize()
        {
            _dataContextMock = new Mock<DataContext>();
            _handlerMock = new Mock<HttpMessageHandler>();
        }

        //[TestMethod]
        //public async Task RetrieveClanInfoScheduler_ShouldReturnMemberAsInactive()
        //{
        //    //arrange
        //    string role = "elder";
        //    DateTime dateTime = DateTime.Now.AddDays(-8);
        //    bool isActive = true;
        //    bool isInClan = true; ;
        //    string format = "";

        //    string formattedDate = dateTime.ToString("yyyyMMddTHHmmss.fffZ");
        //    TestHelperClass helper = new TestHelperClass();
        //    var clanMembers = new List<DbClanMembers>();

        //    //var expectedRecord = helper.GetCurrentRiverRace();
        //    //currentRiverRace.Add(expectedRecord);
         
        //    var mockContext = new Mock<DataContext>();
        //    mockContext.Setup(dc => dc.DbClanMembers).Returns(MockDbSet(clanMembers));
        //    mockContext.Setup(dc => dc.Update(new DbClanMembers()));

        //    var riverRace = new Mock<ClanMemberLogic>(mockContext.Object);

        //    riverRace.Setup(x => x.RoyaleApiCall()).ReturnsAsync(GetOneMember(formattedDate));
        //    riverRace.Setup(x => x.GetAllClanMembers()).Returns(helper.GetClanMember(role, dateTime, isActive, isInClan));
        //    riverRace.Setup(x => x.AddNewMember(new Member(), format));
        //    riverRace.Setup(x => x.AddLog("tag", "name", MemberStatus.Joined, "", ""));

        //    //act
        //    List<DbClanMembers> response = await riverRace.Object.RetrieveClanInfoScheduler();
        //    DbClanMembers member = response.FirstOrDefault();
        //    mockContext.Verify(x => x.SaveChanges(), Times.Once());
        //    Assert.IsFalse(isActive);
        //}

        //[TestMethod]
        //public async Task RetrieveClanInfoScheduler_ShouldReturnMemberActiveAgain()
        //{
        //    //arrange
        //    string role = "elder";
        //    DateTime dateTime = DateTime.Now;
        //    bool isActive = false;
        //    bool isInClan = true; 
        //    string format = "";
        //    string formattedDate = dateTime.ToString("yyyyMMddTHHmmss.fffZ");
        //    TestHelperClass helper = new TestHelperClass();
        //    var clanMembers = new List<DbClanMembers>();

        //    //var expectedRecord = helper.GetCurrentRiverRace();
        //    //currentRiverRace.Add(expectedRecord);

        //    var mockContext = new Mock<DataContext>();
        //    mockContext.Setup(dc => dc.DbClanMembers).Returns(MockDbSet(clanMembers));
        //    mockContext.Setup(dc => dc.Update(new DbClanMembers()));

        //    var riverRace = new Mock<ClanMemberLogic>(mockContext.Object);

        //    riverRace.Setup(x => x.RoyaleApiCall()).ReturnsAsync(GetOneMember(formattedDate));
        //    riverRace.Setup(x => x.GetAllClanMembers()).Returns(helper.GetClanMember(role, dateTime, isActive, isInClan));
        //    riverRace.Setup(x => x.AddNewMember(new Member(), format));
        //    riverRace.Setup(x => x.AddLog("tag", "name", MemberStatus.Joined, "", ""));

        //    //act
        //    List<DbClanMembers> response = await riverRace.Object.RetrieveClanInfoScheduler();
        //    DbClanMembers member = response.FirstOrDefault();
        //    mockContext.Verify(x => x.SaveChanges(), Times.Once());
        //    string newRole = member.Role;
        //    Assert.IsTrue(member.IsActive);
        //}

        //[TestMethod]
        //public async Task RetrieveClanInfoScheduler_ShouldReturnMemberRoleChange()
        //{
        //    //arrange
        //    string role = "member";
        //    DateTime dateTime = DateTime.Now;
        //    bool isActive = true;
        //    bool isInClan = true; 
        //    string format = "";

        //    string formattedDate = dateTime.ToString("yyyyMMddTHHmmss.fffZ");

        //    TestHelperClass helper = new TestHelperClass();
        //    var clanMembers = new List<DbClanMembers>();

        //    //var expectedRecord = helper.GetCurrentRiverRace();
        //    //currentRiverRace.Add(expectedRecord);

        //    var mockContext = new Mock<DataContext>();
        //    mockContext.Setup(dc => dc.DbClanMembers).Returns(MockDbSet(clanMembers));
        //    mockContext.Setup(dc => dc.Update(new DbClanMembers()));

        //    var riverRace = new Mock<ClanMemberLogic>(mockContext.Object);

        //    riverRace.Setup(x => x.RoyaleApiCall()).ReturnsAsync(GetOneMember(formattedDate));
        //    riverRace.Setup(x => x.GetAllClanMembers()).Returns(helper.GetClanMember(role, dateTime, isActive, isInClan));
        //    riverRace.Setup(x => x.AddNewMember(new Member(), format));
        //    riverRace.Setup(x => x.AddLog("tag", "name", MemberStatus.Joined, "", ""));

        //    //act
        //    List<DbClanMembers> response = await riverRace.Object.RetrieveClanInfoScheduler();
        //    DbClanMembers member = response.FirstOrDefault();
        //    mockContext.Verify(x => x.SaveChanges(), Times.Once());
        //    string newRole = member.Role;   
        //    Assert.AreEqual("elder", newRole);
        //    Assert.AreEqual(1, response.Count);
        //}

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

        private string GetString()
        {
            return "{\"items\":[{\"tag\":\"#L2880298\",\"name\":\"zuenhairygranny\",\"role\":\"elder\",\"lastSeen\":\"20231028T165057.000Z\",\"expLevel\":53,\"trophies\":7249,\"arena\":{\"id\":54000016,\"name\":\"Arena 19\"},\"clanRank\":1,\"previousClanRank\":1,\"donations\":84,\"donationsReceived\":0,\"clanChestPoints\":0},{\"tag\":\"#2QPP2JLP8\",\"name\":\"Piet\",\"role\":\"coLeader\",\"lastSeen\":\"20231028T162928.000Z\",\"expLevel\":55,\"trophies\":8380,\"arena\":{\"id\":54000018,\"name\":\"Arena 21\"},\"clanRank\":2,\"previousClanRank\":2,\"donations\":326,\"donationsReceived\":280,\"clanChestPoints\":0},{\"tag\":\"#Y2VPQL0C\",\"name\":\"niekuden\",\"role\":\"elder\",\"lastSeen\":\"20231028T122808.000Z\",\"expLevel\":54,\"trophies\":8170,\"arena\":{\"id\":54000018,\"name\":\"Arena 21\"},\"clanRank\":3,\"previousClanRank\":3,\"donations\":104,\"donationsReceived\":120,\"clanChestPoints\":0},{\"tag\":\"#9JL98CQUY\",\"name\":\"Kingkiller\",\"role\":\"elder\",\"lastSeen\":\"20231028T164108.000Z\",\"expLevel\":42,\"trophies\":7591,\"arena\":{\"id\":54000017,\"name\":\"Arena 20\"},\"clanRank\":4,\"previousClanRank\":4,\"donations\":192,\"donationsReceived\":136,\"clanChestPoints\":0},{\"tag\":\"#VUUYJ9YC\",\"name\":\"britt power\",\"role\":\"elder\",\"lastSeen\":\"20231027T210446.000Z\",\"expLevel\":48,\"trophies\":7530,\"arena\":{\"id\":54000017,\"name\":\"Arena 20\"},\"clanRank\":5,\"previousClanRank\":5,\"donations\":695,\"donationsReceived\":360,\"clanChestPoints\":0}],\"paging\":{\"cursors\":{}}}";
        }

        private string GetOneMember(string dateTime)
        {
            return "{\"items\":[{\"tag\":\"#L2880298\",\"name\":\"zuenhairygranny\",\"role\":\"elder\",\"lastSeen\":\"" + dateTime + "\",\"expLevel\":53,\"trophies\":7249,\"arena\":{\"id\":54000016,\"name\":\"Arena 19\"},\"clanRank\":1,\"previousClanRank\":1,\"donations\":84,\"donationsReceived\":0,\"clanChestPoints\":0}],\"paging\":{\"cursors\":{}}}";
        }
    }
}
