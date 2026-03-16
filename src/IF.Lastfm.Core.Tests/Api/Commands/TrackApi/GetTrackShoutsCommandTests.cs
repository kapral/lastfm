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
            ClassicAssert.AreEqual(_command.Method, "track.getShouts");

            ClassicAssert.AreEqual(_command.Parameters["track"], "Genesis");
            ClassicAssert.AreEqual(_command.Parameters["artist"], "Grimes");
            ClassicAssert.AreEqual(_command.Parameters["autocorrect"], "1");
            ClassicAssert.AreEqual(_command.Parameters["page"], "5");
            ClassicAssert.AreEqual(_command.Parameters["limit"], "7");
            //ClassicAssert.AreEqual(_command.Parameters["disablecachetoken"], "1");
        }

        [Test]
        public async Task HandleSuccessResponse()
        {
            var file = GetFileContents("TrackApi.TrackGetShouts.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(TrackApiResponses.TrackGetShouts));

            var parsed = await _command.HandleResponse(response);

            ClassicAssert.IsTrue(parsed.Success);
            ClassicAssert.IsNotNull(parsed.Content);
            ClassicAssert.IsTrue(parsed.Page == 5);
            ClassicAssert.IsTrue(parsed.Content.Count() == 7);
        }

        [Test]
        public async Task HandleResponseSingle()
        {
            var file = GetFileContents("TrackApi.TrackGetShoutsSingle.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(TrackApiResponses.TrackGetShoutsSingle));

            var parsed = await _command.HandleResponse(response);

            ClassicAssert.IsTrue(parsed.Success);
            ClassicAssert.IsNotNull(parsed.Content);
            ClassicAssert.IsTrue(parsed.Content.Count() == 1);
        }

        [Test]
        public async Task HandleEmptyResponse()
        {
            var file = GetFileContents("TrackApi.TrackGetShoutsEmpty.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(TrackApiResponses.TrackGetShoutsEmpty));

            var parsed = await _command.HandleResponse(response);

            ClassicAssert.IsTrue(parsed.Success);
            ClassicAssert.IsNotNull(parsed.Content);
            ClassicAssert.IsTrue(!parsed.Content.Any());
        }

        [Test]
        public async Task HandleErrorResponse()
        {
            var file = GetFileContents("TrackApi.TrackGetShoutsError.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(TrackApiResponses.TrackGetShoutsError));

            var parsed = await _command.HandleResponse(response);

            ClassicAssert.IsFalse(parsed.Success);
            ClassicAssert.IsTrue(parsed.Status == LastResponseStatus.MissingParameters);
            ClassicAssert.IsNotNull(parsed.Content);
            ClassicAssert.IsTrue(!parsed.Content.Any());
        }
    }
}
