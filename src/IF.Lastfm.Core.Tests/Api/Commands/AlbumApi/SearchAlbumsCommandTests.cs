using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Tests.Resources;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.Album;
using NUnit.Framework;

namespace IF.Lastfm.Core.Tests.Api.Commands.AlbumApi
{
    public class SearchAlbumsCommandTests : CommandTestsBase
    {
        private SearchCommand _command;

        public SearchAlbumsCommandTests()
        {
            _command = new SearchCommand(MAuth.Object, "By the throat")
                       {
                           Page = 2,
                           Count = 3
                       };

            _command.SetParameters();
        }

        [Test]
        public void Constructor()
        {
            Assert.That(_command.Method, Is.EqualTo("album.search"));

            Assert.That(_command.Parameters["album"], Is.EqualTo("By the throat"));
            Assert.That(_command.Parameters["page"], Is.EqualTo("2"));
            Assert.That(_command.Parameters["limit"], Is.EqualTo("3"));
        }

        [Test]
        public async Task HandleSuccessResponse()
        {
            var file = GetFileContents("AlbumApi.AlbumSearch.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(AlbumApiResponses.AlbumSearch));

            var parsed = await _command.HandleResponse(response);

            Assert.That(parsed.Success);
            Assert.That(parsed.Content, Is.Not.Null);
            Assert.That(parsed.Page, Is.EqualTo(2));
            Assert.That(parsed.Content.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task HandleResponseSingle()
        {
            var file = GetFileContents("AlbumApi.AlbumSearchSingle.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(AlbumApiResponses.AlbumSearchSingle));

            var parsed = await _command.HandleResponse(response);

            Assert.That(parsed.Success);
            Assert.That(parsed.Content, Is.Not.Null);
            Assert.That(parsed.Content.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task HandleEmptyResponse()
        {
            var file = GetFileContents("AlbumApi.AlbumSearchEmpty.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(AlbumApiResponses.AlbumSearchEmpty));

            var parsed = await _command.HandleResponse(response);

            Assert.That(parsed.Success);
            Assert.That(parsed.Content, Is.Not.Null);
            Assert.That(!parsed.Content.Any());
        }

        [Test]
        public async Task HandleErrorResponse()
        {
            var file = GetFileContents("AlbumApi.AlbumSearchError.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(AlbumApiResponses.AlbumSearchError));

            var parsed = await _command.HandleResponse(response);

            Assert.That(parsed.Success, Is.False);
            Assert.That(parsed.Status, Is.EqualTo(LastResponseStatus.MissingParameters));
            Assert.That(parsed.Content, Is.Not.Null);
            Assert.That(!parsed.Content.Any());
        }
    }
}
