using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Tests.Resources;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.Album;
using NUnit.Framework;

namespace IF.Lastfm.Core.Tests.Api.Commands.AlbumApi
{
    public class GetAlbumTopTagsCommandTest : CommandTestsBase
    {
        private GetTopTagsCommand _command;

        public GetAlbumTopTagsCommandTest()
        {
            _command = new GetTopTagsCommand(MAuth.Object)
            {
                AlbumName = "Believe",
                ArtistName = "Cher"
            };

            _command.SetParameters();
        }

        [Test]
        public void Constructor()
        {
            ClassicAssert.AreEqual(_command.Method, "album.getTopTags");
            ClassicAssert.AreEqual(_command.Parameters["album"], "Believe");
            ClassicAssert.AreEqual(_command.Parameters["artist"], "Cher");
        }

        [Test]
        public async Task HandleSuccessResponse()
        {
            var file = GetFileContents("AlbumApi.AlbumGetTopTags.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(AlbumApiResponses.AlbumGetTopTags));

            var parsed = await _command.HandleResponse(response);

            ClassicAssert.IsTrue(parsed.Success);
            ClassicAssert.IsNotNull(parsed.Content);
        }

        [Test]
        public async Task HandleEmptyResponse()
        {
            var file = GetFileContents("AlbumApi.AlbumGetTopTagsEmpty.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(AlbumApiResponses.AlbumGetTopTagsEmpty));

            var parsed = await _command.HandleResponse(response);

            ClassicAssert.IsTrue(parsed.Success);
            ClassicAssert.IsNotNull(parsed.Content);
            ClassicAssert.IsTrue(!parsed.Content.Any());
        }

        [Test]
        public async Task HandleErrorResponse()
        {
            var file = GetFileContents("AlbumApi.AlbumGetTopTagsError.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(AlbumApiResponses.AlbumGetTopTagsError));

            var parsed = await _command.HandleResponse(response);

            ClassicAssert.IsFalse(parsed.Success);
            ClassicAssert.IsTrue(parsed.Status == LastResponseStatus.MissingParameters);
            ClassicAssert.IsNotNull(parsed.Content);
            ClassicAssert.IsTrue(!parsed.Content.Any());
        }
    }
}
