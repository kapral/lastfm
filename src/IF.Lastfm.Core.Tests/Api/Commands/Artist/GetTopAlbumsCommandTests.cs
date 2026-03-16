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
            ClassicAssert.IsTrue(_commandArtist.Parameters.TryGetValue("artist", out artistValue));
            ClassicAssert.AreEqual("Steely Dan", artistValue);

            var file = GetFileContents("ArtistApi.ArtistGetTopAlbumsSuccess.json");
            var response = CreateResponseMessage(file);
            var parsed = await _commandArtist.HandleResponse(response);
            
            ClassicAssert.IsTrue(parsed.Success, "parsed.success should be true");
            ClassicAssert.AreEqual(10, parsed.Content.Count);
            ClassicAssert.AreEqual(10, parsed.PageSize);
            ClassicAssert.AreEqual(16897, parsed.TotalItems);
            ClassicAssert.AreEqual(1690, parsed.TotalPages);
            ClassicAssert.AreEqual("Steely Dan", parsed.Content[0].ArtistName);
        }

        [Test]
        public void GetTopAlbums_ByMbid_Success()
        {
            _commandMbid.SetParameters();
            string mbidValue;
            ClassicAssert.IsTrue(_commandMbid.Parameters.TryGetValue("mbid", out mbidValue));
            ClassicAssert.AreEqual("e01c3376-15fa-40d7-b747-5f219bdefdd7", mbidValue);
        }
        
        [Test]
        public async Task GetTopAlbums_HandleResponse_Missing()
        {
            var file = GetFileContents("ArtistApi.ArtistGetTopAlbumsMissing.json");
            var response = CreateResponseMessage(file);
            
            var parsed = await _commandArtist.HandleResponse(response);

            ClassicAssert.IsFalse(parsed.Success, "parsed.success should be false");
            ClassicAssert.AreEqual(LastResponseStatus.MissingParameters, parsed.Status);
        }
    }
}
