using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Tests.Resources;
using NUnit.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.Track;

namespace IF.Lastfm.Core.Tests.Api.Commands.TrackApi
{

    public class GetTrackShoutsCommandTests : CommandTestsBase
    {
        private GetShoutsCommand _command;

        public GetTrackShoutsCommandTests()
        {
            _command = new GetShoutsCommand(MAuth.Object, "Genesis", "Grimes")
                       {
                           Autocorrect = true,
                           Page = 5,
                           Count = 7
                       };

            _command.SetParameters();
        }

        [Test]
        public void Constructor()
        {
            Assert.That(_command.Method, Is.EqualTo("track.getShouts"));

            Assert.That(_command.Parameters["track"], Is.EqualTo("Genesis"));
            Assert.That(_command.Parameters["artist"], Is.EqualTo("Grimes"));
            Assert.That(_command.Parameters["autocorrect"], Is.EqualTo("1"));
            Assert.That(_command.Parameters["page"], Is.EqualTo("5"));
            Assert.That(_command.Parameters["limit"], Is.EqualTo("7"));
            //Assert.That(_command.Parameters["disablecachetoken"], Is.EqualTo("1"));
        }

        [Test]
        public async Task HandleSuccessResponse()
        {
            var file = GetFileContents("TrackApi.TrackGetShouts.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(TrackApiResponses.TrackGetShouts));

            var parsed = await _command.HandleResponse(response);

            Assert.That(parsed.Success);
            Assert.That(parsed.Content, Is.Not.Null);
            Assert.That(parsed.Page, Is.EqualTo(5));
            Assert.That(parsed.Content.Count(), Is.EqualTo(7));
        }

        [Test]
        public async Task HandleResponseSingle()
        {
            var file = GetFileContents("TrackApi.TrackGetShoutsSingle.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(TrackApiResponses.TrackGetShoutsSingle));

            var parsed = await _command.HandleResponse(response);

            Assert.That(parsed.Success);
            Assert.That(parsed.Content, Is.Not.Null);
            Assert.That(parsed.Content.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task HandleEmptyResponse()
        {
            var file = GetFileContents("TrackApi.TrackGetShoutsEmpty.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(TrackApiResponses.TrackGetShoutsEmpty));

            var parsed = await _command.HandleResponse(response);

            Assert.That(parsed.Success);
            Assert.That(parsed.Content, Is.Not.Null);
            Assert.That(!parsed.Content.Any());
        }

        [Test]
        public async Task HandleErrorResponse()
        {
            var file = GetFileContents("TrackApi.TrackGetShoutsError.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(TrackApiResponses.TrackGetShoutsError));

            var parsed = await _command.HandleResponse(response);

            Assert.That(parsed.Success, Is.False);
            Assert.That(parsed.Status, Is.EqualTo(LastResponseStatus.MissingParameters));
            Assert.That(parsed.Content, Is.Not.Null);
            Assert.That(!parsed.Content.Any());
        }
    }
}
