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
            Assert.That(_command.Method, Is.EqualTo("album.getTopTags"));
            Assert.That(_command.Parameters["album"], Is.EqualTo("Believe"));
            Assert.That(_command.Parameters["artist"], Is.EqualTo("Cher"));
        }

        [Test]
        public async Task HandleSuccessResponse()
        {
            var file = GetFileContents("AlbumApi.AlbumGetTopTags.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(AlbumApiResponses.AlbumGetTopTags));

            var parsed = await _command.HandleResponse(response);

            Assert.That(parsed.Success);
            Assert.That(parsed.Content, Is.Not.Null);
        }

        [Test]
        public async Task HandleEmptyResponse()
        {
            var file = GetFileContents("AlbumApi.AlbumGetTopTagsEmpty.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(AlbumApiResponses.AlbumGetTopTagsEmpty));

            var parsed = await _command.HandleResponse(response);

            Assert.That(parsed.Success);
            Assert.That(parsed.Content, Is.Not.Null);
            Assert.That(!parsed.Content.Any());
        }

        [Test]
        public async Task HandleErrorResponse()
        {
            var file = GetFileContents("AlbumApi.AlbumGetTopTagsError.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(AlbumApiResponses.AlbumGetTopTagsError));

            var parsed = await _command.HandleResponse(response);

            Assert.That(parsed.Success, Is.False);
            Assert.That(parsed.Status, Is.EqualTo(LastResponseStatus.MissingParameters));
            Assert.That(parsed.Content, Is.Not.Null);
            Assert.That(!parsed.Content.Any());
        }
    }
}
