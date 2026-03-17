using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.User;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Api.Helpers;
using IF.Lastfm.Core.Objects;
using IF.Lastfm.Core.Tests.Resources;
using NUnit.Framework;

namespace IF.Lastfm.Core.Tests.Api.Commands
{
    public class UserGetTopArtistsCommandTests : CommandTestsBase
    {
        private const string USER = "Aurvandil";
        private const LastStatsTimeSpan SPAN = LastStatsTimeSpan.Overall;

        [Test]
        public void ParametersCorrect()
        {
            var expected = new Dictionary<string, string>
            {
                {"user", USER},
                {"period", SPAN.GetApiName()},
                {"page", "5"},
                {"limit", "10"}
            };

            var command = new GetTopArtistsCommand(MAuth.Object, USER, SPAN)
            {
                Page = 5,
                Count = 10
            };

            command.SetParameters();
            command.Parameters.Remove("disablecachetoken");

            TestHelper.AssertSerialiseEqual(expected, command.Parameters);
        }

        [Test]
        public async Task HandleResponseSingle()
        {
            var command = new GetTopArtistsCommand(MAuth.Object, USER, SPAN)
            {
                Page = 1,
                Count = 1
            };

            command.SetParameters();

            var expectedArtist = new LastArtist
            {
                Name = "Anathema",
                PlayCount = 5216,
                Mbid = "20aa23e3-3532-42ca-acf6-e8c2e9df2688",
                Url = new Uri("http://www.last.fm/music/Anathema")
            };

            var file = GetFileContents("UserApi.UserGetTopArtistsSingle.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(UserApiResponses.UserGetTopArtistsSingle));
            var parsed = await command.HandleResponse(response);

            Assert.That(parsed.Success);
            Assert.That(parsed.Page, Is.EqualTo(1));
            Assert.That(parsed.PageSize, Is.EqualTo(1));
            Assert.That(parsed.TotalItems, Is.EqualTo(1124));
            Assert.That(parsed.TotalPages, Is.EqualTo(1124));
            Assert.That(parsed.Content.Count, Is.EqualTo(1));

            var actualArtist = parsed.Content.First();

            TestHelper.AssertSerialiseEqual(expectedArtist, actualArtist);
        }

        [Test]
        public async Task HandleResponseMultiple()
        {
            var command = new GetTopArtistsCommand(MAuth.Object, USER, SPAN)
            {
                Page = 1,
                Count = 2
            };

            command.SetParameters();

            var expectedArtists = new List<LastArtist>
            {
                new LastArtist
                {
                    Name = "Anathema",
                    PlayCount = 5216,
                    Mbid = "20aa23e3-3532-42ca-acf6-e8c2e9df2688",
                    Url = new Uri("http://www.last.fm/music/Anathema")
                },
                new LastArtist
                {
                    Name = "Insomnium",
                    PlayCount = 4670,
                    Mbid = "c1f8e226-75ea-4fe6-83ce-59c122bcbca4",
                    Url = new Uri("http://www.last.fm/music/Insomnium")
                },
            };

            var file = GetFileContents("UserApi.UserGetTopArtistsMultiple.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(UserApiResponses.UserGetTopArtistsMultiple));
            var parsed = await command.HandleResponse(response);

            Assert.That(parsed.Success);
            Assert.That(parsed.Page, Is.EqualTo(1));
            Assert.That(parsed.PageSize, Is.EqualTo(2));
            Assert.That(parsed.TotalItems, Is.EqualTo(1124));
            Assert.That(parsed.TotalPages, Is.EqualTo(562));
            Assert.That(parsed.Content.Count, Is.EqualTo(2));

            var actualArtists = parsed.Content;

            TestHelper.AssertSerialiseEqual(expectedArtists, actualArtists);
        }

        [Test]
        public async Task HandleResponseEmpty()
        {
            const string USER_WITH_NO_PLAYS = "e";
            var command = new GetTopArtistsCommand(MAuth.Object, USER_WITH_NO_PLAYS, SPAN)
            {
                Page = 1,
                Count = 1
            };

            command.SetParameters();

            var file = GetFileContents("UserApi.UserGetTopArtistsEmpty.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(UserApiResponses.UserGetTopArtistsEmpty));
            var parsed = await command.HandleResponse(response);

            Assert.That(parsed.Success);
            Assert.That(parsed.Page, Is.EqualTo(1));
            Assert.That(parsed.PageSize, Is.EqualTo(0));
            Assert.That(parsed.TotalItems, Is.EqualTo(0));
            Assert.That(parsed.TotalPages, Is.EqualTo(1));
            Assert.That(parsed.Content.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task HandleResponseError()
        {
            var command = new GetTopArtistsCommand(MAuth.Object, USER, SPAN)
            {
                Page = 1,
                Count = 1
            };

            command.SetParameters();

            var file = GetFileContents("UserApi.UserGetTopArtistsError.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(UserApiResponses.UserGetTopArtistsError));
            var parsed = await command.HandleResponse(response);

            Assert.That(parsed.Success, Is.False);
            Assert.That(parsed.Page, Is.EqualTo(1));
            Assert.That(parsed.PageSize, Is.EqualTo(0));
            Assert.That(parsed.TotalItems, Is.EqualTo(0));
            Assert.That(parsed.TotalPages, Is.EqualTo(1));
            Assert.That(parsed.Content.Count, Is.EqualTo(0));
            Assert.That(parsed.Status, Is.EqualTo(LastResponseStatus.MissingParameters));
        }
    }
}
