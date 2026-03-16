using IF.Lastfm.Core.Api.Helpers;
using NUnit.Framework;

namespace IF.Lastfm.Core.Tests.Api.Helpers
{
    internal enum TestApiEnum
    {
        Unknown = 0,

        [ApiName("dogs")]
        Dogs,

        [ApiName("cats")]
        Cats
    }


    public class ApiHelperTests
    {
        [Test]
        public void GetApiNameReturnsAttribute()
        {
            var enumValue = TestApiEnum.Dogs;

            var expected = "dogs";
            var actual = enumValue.GetApiName();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GetApiNameReturnsValueIfNoAttribute()
        {
            var enumValue = TestApiEnum.Unknown;

            var expected = "Unknown";
            var actual = enumValue.GetApiName();

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
