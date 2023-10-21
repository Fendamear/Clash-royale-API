using Castle.Core.Configuration;
using ClashRoyaleApi.Data;
using ClashRoyaleApi.Logic.ClanMembers;
using ClashRoyaleApi.Logic.RoyaleApi;
using Microsoft.AspNetCore.Hosting;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        //public async Task Test1()
        //{
        //   //arrange
        //    _handlerMock
        //        .Protected()
        //        .Setup<Task<HttpResponseMessage>>(
        //            "SendAsync",
        //            ItExpr.IsAny<HttpRequestMessage>(),
        //            ItExpr.IsAny<CancellationToken>()
        //        )
        //        .ReturnsAsync( new HttpResponseMessage
        //        {
        //            StatusCode = System.Net.HttpStatusCode.OK,
        //            Content = new StringContent(File.ReadAllText(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "claninfo.json")))
        //        } );

        //    var httpClient = new HttpClient(_handlerMock.Object);
        //    var httpClientWrapper = new HttpClientWrapper(httpClient);

        //    _logic = new ClanMemberLogic(_dataContextMock.Object, httpClientWrapper);

        //    await _logic.RetrieveClanInfoScheduler();

        //}








    }
}
