using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.Artist;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Objects;
using IF.Lastfm.Core.Tests.Resources;
using NUnit.Framework;

namespace IF.Lastfm.Core.Tests.Api.Commands.Artist
{
    public class GetTopAlbumsCommandTests : CommandTestsBase
    {
        private GetTopAlbumsCommand _commandArtist;
        private GetTopAlbumsCommand _commandMbid;

        [SetUp]
        public void Initialise()
        {
            _commandArtist = new GetTopAlbumsCommand(MAuth.Object)
            {
                ArtistName = "Steely Dan"
            };
            _commandMbid = new GetTopAlbumsCommand(MAuth.Object)
            {
                ArtistMbid = "e01c3376-15fa-40d7-b747-5f219bdefdd7"
            };
        }

        [Test]
        public async Task GetTopAlbums_HandleResponse_Success()
        {
            _commandArtist.SetParameters();
            string artistValue;
            Assert.That(_commandArtist.Parameters.TryGetValue("artist", out artistValue));
            Assert.That(artistValue, Is.EqualTo("Steely Dan"));

            var file = GetFileContents("ArtistApi.ArtistGetTopAlbumsSuccess.json");
            var response = CreateResponseMessage(file);
            var parsed = await _commandArtist.HandleResponse(response);

            Assert.That(parsed.Success, Is.True, "parsed.success should be true");
            Assert.That(parsed.Content.Count, Is.EqualTo(10));
            Assert.That(parsed.PageSize, Is.EqualTo(10));
            Assert.That(parsed.TotalItems, Is.EqualTo(16897));
            Assert.That(parsed.TotalPages, Is.EqualTo(1690));
            Assert.That(parsed.Content[0].ArtistName, Is.EqualTo("Steely Dan"));
        }

        [Test]
        public void GetTopAlbums_ByMbid_Success()
        {
            _commandMbid.SetParameters();
            string mbidValue;
            Assert.That(_commandMbid.Parameters.TryGetValue("mbid", out mbidValue));
            Assert.That(mbidValue, Is.EqualTo("e01c3376-15fa-40d7-b747-5f219bdefdd7"));
        }

        [Test]
        public async Task GetTopAlbums_HandleResponse_Missing()
        {
            var file = GetFileContents("ArtistApi.ArtistGetTopAlbumsMissing.json");
            var response = CreateResponseMessage(file);

            var parsed = await _commandArtist.HandleResponse(response);

            Assert.That(parsed.Success, Is.False, "parsed.success should be false");
            Assert.That(parsed.Status, Is.EqualTo(LastResponseStatus.MissingParameters));
        }
    }
}
