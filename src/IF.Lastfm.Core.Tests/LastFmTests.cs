using System;
using System.Collections.Generic;
using System.Text;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Tests.Resources;
using NUnit.Framework;
using Moq;
using System.Reflection;
using System.IO;

namespace IF.Lastfm.Core.Tests
{
    public class LastFmTests
    {
        [Test]
        public void ApiUrlFormatReturnsCorrectly()
        {
            const string expected = "https://ws.audioscrobbler.com/2.0/?method=tobias.funke&api_key=suddenvalley&blue=performance&format=json&uncle=t-bag";

            var actual = LastFm.FormatApiUrl("tobias.funke", "suddenvalley", new Dictionary<string, string>
                {
                    {"uncle", "t-bag"},
                    {"blue", "performance"}
                }, true);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void IsResponseValid()
        {
            LastResponseStatus status;

            Assert.That(LastFm.IsResponseValid(null, out status), Is.False);
            Assert.That(LastFm.IsResponseValid("{invalid json", out status), Is.False);

            var error6 = GetFileContents("ArtistApi.ArtistGetTagsError.json");
            //var error6 = Encoding.UTF8.GetString(ArtistApiResponses.ArtistGetTagsError);
            Assert.That(LastFm.IsResponseValid(error6, out status), Is.False);
            Assert.That(status, Is.EqualTo(LastResponseStatus.MissingParameters));

            string message;
            Assert.That(LastFm.IsResponseValid(error6, out status, out message), Is.False);
            Assert.That(status, Is.EqualTo(LastResponseStatus.MissingParameters));
            Assert.That(message, Is.EqualTo("Invalid user supplied"));

            var goodResponse = GetFileContents("ArtistApi.ArtistGetInfoSuccess.json");
            //var goodResponse = Encoding.UTF8.GetString(ArtistApiResponses.ArtistGetInfoSuccess);
            Assert.That(LastFm.IsResponseValid(goodResponse, out status));
            Assert.That(status, Is.EqualTo(LastResponseStatus.Successful));
        }

        protected string GetFileContents(string sampleFile)
        {
            var asm = Assembly.GetExecutingAssembly();
            var resource = string.Format("IF.Lastfm.Core.Tests.Resources.{0}", sampleFile);
            using (var stream = asm.GetManifestResourceStream(resource))
            {
                if (stream != null)
                {
                    var reader = new StreamReader(stream);
                    return reader.ReadToEnd();
                }
            }
            return string.Empty;
        }
    }
}
