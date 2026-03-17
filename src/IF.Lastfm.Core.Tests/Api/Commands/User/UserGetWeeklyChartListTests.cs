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

            Assert.That(parsed.Success, Is.False);
            Assert.That(parsed.Status, Is.EqualTo(LastResponseStatus.MissingParameters));
        }

        [Test]
        public async Task GetWeeklyChartList_HandleResponse_Success()
        {
            var file = GetFileContents("UserApi.UserGetWeeklyChartList.json");
            var response = CreateResponseMessage(file);
            var parsed = await _command.HandleResponse(response);

            Assert.That(parsed.Success);
            Assert.That(parsed.Content.Count, Is.EqualTo(612));

            //convert dates back to unix time
            var lastPeriod = parsed.Content.Last();

            Assert.That(lastPeriod.From.ToString(), Is.EqualTo("1546171200"));
            Assert.That(lastPeriod.To.ToString(), Is.EqualTo("1546776000"));
            Assert.That(lastPeriod.FromDate, Is.EqualTo(new DateTime(2018,12,30,12,0,0)));
            Assert.That(lastPeriod.ToDate, Is.EqualTo(new DateTime(2019,1,6,12,0,0)));
        }

    }
}
