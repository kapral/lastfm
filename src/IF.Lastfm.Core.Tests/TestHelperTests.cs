using System;
using NUnit.Framework;

namespace IF.Lastfm.Core.Tests
{
    public class TestHelperTests
    {
        [Test]
        public void RoundsToNearestSecond()
        {
            var now = new DateTimeOffset(2015, 03, 04, 20, 07, 21, TimeSpan.Zero);

            var testDto1 = now.AddMilliseconds(450);
            var actualDto1 = testDto1.RoundToNearestSecond();

            Assert.That(actualDto1, Is.EqualTo(now));
            TestHelper.AssertSerialiseEqual(now, actualDto1);

            var expectedDto2 = now.AddSeconds(1);
            var testDto2 = now.AddMilliseconds(550);
            var actualDto2 = testDto2.RoundToNearestSecond();

            Assert.That(actualDto2, Is.EqualTo(expectedDto2));
            TestHelper.AssertSerialiseEqual(expectedDto2, actualDto2);
        }
    }
}
