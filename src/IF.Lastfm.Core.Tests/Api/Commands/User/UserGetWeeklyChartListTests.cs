using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api;
using IF.Lastfm.Core.Api.Commands.User;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using IF.Lastfm.Core.Tests.Resources;
using NUnit.Framework;
using Moq;

namespace IF.Lastfm.Core.Tests.Api.Commands
{
    public class UserGetWeeklyChartListsTests : CommandTestsBase
    {
        private const string user = "test";

        private GetWeeklyChartListCommand _command;
        private Mock<ILastAuth> _mockAuth;

        [SetUp]
        public void TestInitialise()
        {
            _mockAuth = new Mock<ILastAuth>();
            _command = new GetWeeklyChartListCommand(_mockAuth.Object, user)
            {
                //no parameters
             };

            _command.SetParameters();
        }

        [Test]
        public void CorrectParameters()
        {
            var expected = new Dictionary<string, string>
            {
                {"user", user},
                {"disablecachetoken", ""}
            };

            _command.Parameters["disablecachetoken"] = "";

            TestHelper.AssertSerialiseEqual(expected, _command.Parameters);
        }

        [Test]
        public async Task HandleErrorResponse()
        {
            var file = GetFileContents("UserApi.UserGetTopAlbumsError.json");
            var response = CreateResponseMessage(file);
            var parsed = await _command.HandleResponse(response);

            ClassicAssert.IsFalse(parsed.Success);
            ClassicAssert.AreEqual(LastResponseStatus.MissingParameters, parsed.Status);
        }

        [Test]
        public async Task GetWeeklyChartList_HandleResponse_Success()
        {
            var file = GetFileContents("UserApi.UserGetWeeklyChartList.json");
            var response = CreateResponseMessage(file);
            var parsed = await _command.HandleResponse(response);
            
            ClassicAssert.IsTrue(parsed.Success);
            ClassicAssert.AreEqual(612, parsed.Content.Count);

            //convert dates back to unix time
            var lastPeriod = parsed.Content.Last();

            ClassicAssert.AreEqual("1546171200", lastPeriod.From.ToString());
            ClassicAssert.AreEqual("1546776000", lastPeriod.To.ToString());
            ClassicAssert.AreEqual(new DateTime(2018,12,30,12,0,0), lastPeriod.FromDate);
            ClassicAssert.AreEqual(new DateTime(2019,1,6,12,0,0), lastPeriod.ToDate);
        }

    }
}