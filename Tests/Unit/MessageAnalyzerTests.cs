using System;
using System.Linq;
using Application;
using NUnit.Framework;

namespace Tests.Unit
{
    [TestFixture]
    [Parallelizable]
    public class MessageAnalyzerTests
    {
        private MessageAnalyzer _analyzer;

        [OneTimeSetUp]
        public void OneTime()
        {
            _analyzer = new MessageAnalyzer();
        }

        [Test]
        public void MessageRatePerSecond_FutureStartTime_ThrowsException()
        {
            var futureTime = DateTime.UtcNow.AddMinutes(1);
            Assert.Throws<ArgumentException>(() => _analyzer.MessageRatePerSecond(futureTime, 10));
        }

        [Test]
        public void MessageRatePerSecond_ZeroMessageCount_ThrowsException()
        {
            var startTime = DateTime.UtcNow.AddMinutes(-3);
            Assert.Throws<ArgumentException>(() => _analyzer.MessageRatePerSecond(startTime, 0));
        }

        [Test]
        public void MessageRatePerSecond_OneMinuteAgo_ReturnsCorrectRate()
        {
            var startTime = DateTime.UtcNow.AddMinutes(-1);
            var result = _analyzer.MessageRatePerSecond(startTime, 60);
            //the result is rounded here to do a "fuzzy" match since the runtime cannot execute in exactly 1 minute
            Assert.AreEqual(1, Math.Round(result));
        }

        [Test]
        public void GetHashtagsFromMessage_NoHashTags_ReturnsEmpty()
        {
            var message = "no hashtags here";

            var result = _analyzer.GetHashtagsFromMessage(message);

            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void GetHashtagsFromMessage_OneTag_ReturnsHashtag()
        {
            var message = "one #hashtag here";

            var result = _analyzer.GetHashtagsFromMessage(message);

            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public void GetHashtagsFromMessage_MultipleTags_ReturnsMultipleHashtags()
        {
            var message = "#multiple #hashtags here for you to #enjoy";

            var result = _analyzer.GetHashtagsFromMessage(message);

            Assert.AreEqual(3, result.Count);
        }

        [Test]
        [TestCase("no urls here", ExpectedResult = false)]
        [TestCase("one http://domain.com url here", ExpectedResult = true)]
        [TestCase("one http://www.domain.com url here", ExpectedResult = true)]
        [TestCase("one https://domain.com url here", ExpectedResult = true)]
        [TestCase("one https://www.domain.com url here", ExpectedResult = true)]
        [TestCase("multiple http://domain.com url here", ExpectedResult = true)]
        [TestCase("http://domain.com", ExpectedResult = true)]
        [TestCase("http://domain.com and https://www.domain.com", ExpectedResult = true)]
        public bool DoesContainUrl_Tests(string message)
        {
            return _analyzer.DoesContainUrl(message);
        }

        [Test]
        [TestCase("no urls here", ExpectedResult = false)]
        [TestCase("one http://domain.com url here", ExpectedResult = false)]
        [TestCase("one http://pic.twitter.com url here", ExpectedResult = true)]
        [TestCase("one https://pic.twitter.com url here", ExpectedResult = true)]
        [TestCase("http://pic.twitter.com url here", ExpectedResult = true)]
        [TestCase("one http://pic.twitter.com", ExpectedResult = true)]
        [TestCase("one http://pic.twitter.com url here https://pic.twitter.com", ExpectedResult = true)]
        public bool DoesContainPhotoUrl_Tests(string message)
        {
            return _analyzer.DoesContainPhotoUrl(message);
        }

        [Test]
        public void GetDomainsFromMessage_NoUrls_ReturnsEmpty()
        {
            var message = "no urls here";

            var result = _analyzer.GetDomainsFromMessage(message);

            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void GetDomainsFromMessage_OneUrl_ReturnsDomain()
        {
            var message = "one http://domain.com url here";

            var result = _analyzer.GetDomainsFromMessage(message);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("domain.com", result.First());
        }

        [Test]
        public void GetDomainsFromMessage_MultipleUrls_ReturnsMultipleDomain()
        {
            var message = "https://domain.com multiple http://domain1.com urls http://www.domain2.com here";

            var result = _analyzer.GetDomainsFromMessage(message);

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("domain.com", result[0]);
            Assert.AreEqual("domain1.com", result[1]);
            Assert.AreEqual("www.domain2.com", result[2]);
        }
    }
}
