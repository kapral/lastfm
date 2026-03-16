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
            ClassicAssert.AreEqual(_command.Method, "album.search");

            ClassicAssert.AreEqual(_command.Parameters["album"], "By the throat");
            ClassicAssert.AreEqual(_command.Parameters["page"], "2");
            ClassicAssert.AreEqual(_command.Parameters["limit"], "3");
        }

        [Test]
        public async Task HandleSuccessResponse()
        {
            var file = GetFileContents("AlbumApi.AlbumSearch.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(AlbumApiResponses.AlbumSearch));

            var parsed = await _command.HandleResponse(response);

            ClassicAssert.IsTrue(parsed.Success);
            ClassicAssert.IsNotNull(parsed.Content);
            ClassicAssert.IsTrue(parsed.Page == 2);
            ClassicAssert.IsTrue(parsed.Content.Count() == 3);
        }

        [Test]
        public async Task HandleResponseSingle()
        {
            var file = GetFileContents("AlbumApi.AlbumSearchSingle.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(AlbumApiResponses.AlbumSearchSingle));

            var parsed = await _command.HandleResponse(response);

            ClassicAssert.IsTrue(parsed.Success);
            ClassicAssert.IsNotNull(parsed.Content);
            ClassicAssert.IsTrue(parsed.Content.Count() == 1);
        }

        [Test]
        public async Task HandleEmptyResponse()
        {
            var file = GetFileContents("AlbumApi.AlbumSearchEmpty.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(AlbumApiResponses.AlbumSearchEmpty));

            var parsed = await _command.HandleResponse(response);

            ClassicAssert.IsTrue(parsed.Success);
            ClassicAssert.IsNotNull(parsed.Content);
            ClassicAssert.IsTrue(!parsed.Content.Any());
        }

        [Test]
        public async Task HandleErrorResponse()
        {
            var file = GetFileContents("AlbumApi.AlbumSearchError.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(AlbumApiResponses.AlbumSearchError));

            var parsed = await _command.HandleResponse(response);

            ClassicAssert.IsFalse(parsed.Success);
            ClassicAssert.IsTrue(parsed.Status == LastResponseStatus.MissingParameters);
            ClassicAssert.IsNotNull(parsed.Content);
            ClassicAssert.IsTrue(!parsed.Content.Any());
        }
    }
}
