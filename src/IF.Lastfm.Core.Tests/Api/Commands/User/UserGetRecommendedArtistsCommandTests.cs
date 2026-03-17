using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.User;
using IF.Lastfm.Core.Objects;
using IF.Lastfm.Core.Tests.Resources;
using NUnit.Framework;

namespace IF.Lastfm.Core.Tests.Api.Commands
{

    public class UserGetRecommendedArtistsCommandTests : CommandTestsBase
    {
        private GetRecommendedArtistsCommand _commmand;

        [SetUp]
        public void Initialise()
        {
            _commmand = new GetRecommendedArtistsCommand(MAuth.Object);
        }

        [Test]
        public async Task HandleResponseSingle()
        {
            var expectedArtist = new LastArtist
            {
                Name = "Liars",
                Mbid = "03098741-08b3-4dd7-b3f6-1b0bfa2c879c",
                Url = new Uri("http://www.last.fm/music/Liars")
            };

            var file = GetFileContents("UserApi.UserGetRecommendedArtistsSingle.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(UserApiResponses.UserGetRecommendedArtistsSingle));
            var parsed = await _commmand.HandleResponse(response);

            Assert.That(parsed.Success);

            var expectedJson = expectedArtist.WrapEnumerable().TestSerialise();
            var actualJson = parsed.Content.TestSerialise();

            Assert.That(actualJson, Is.EqualTo(expectedJson), expectedJson.DifferencesTo(actualJson));
        }

        [Test]
        public async Task HandleResponseMultiple()
        {
            var expectedArtists = new List<LastArtist>
            {
                new LastArtist
                {
                    Name = "Liars",
                    Mbid = "03098741-08b3-4dd7-b3f6-1b0bfa2c879c",
                    Url = new Uri("http://www.last.fm/music/Liars")
                },
                new LastArtist
                {
                    Name = "The Haxan Cloak",
                    Mbid = "c9224968-d1b7-455f-84f4-2ceefa7d3a4e",
                    Url = new Uri("http://www.last.fm/music/The+Haxan+Cloak")
                },
                new LastArtist
                {
                    Name = "Cloetta Paris",
                    Mbid = "24a9af30-cb7a-4456-ba3d-6daba1245b26",
                    Url = new Uri("http://www.last.fm/music/Cloetta+Paris")
                },
            };

            var file = GetFileContents("UserApi.UserGetRecommendedArtistsMultiple.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(UserApiResponses.UserGetRecommendedArtistsMultiple));
            var parsed = await _commmand.HandleResponse(response);

            Assert.That(parsed.Success);

            var expectedJson = expectedArtists.TestSerialise();
            var actualJson = parsed.Content.TestSerialise();

            Assert.That(actualJson, Is.EqualTo(expectedJson), expectedJson.DifferencesTo(actualJson));
        }
    }
}
