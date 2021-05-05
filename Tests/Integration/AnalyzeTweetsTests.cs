using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features;
using Application.Interfaces;
using MediatR;
using Moq;
using NUnit.Framework;

namespace Tests.Integration
{
    [TestFixture]
    [Parallelizable]
    public class AnalyzeTweetsTests
    {
        private Mock<IRepository> _store;
        private Mock<IMessageAnalyzer> _analyzer;
        private Mock<IEmojiService> _emojiService;

        [OneTimeSetUp]
        public void OneTime()
        {
            _store = new Mock<IRepository>();
            _analyzer = new Mock<IMessageAnalyzer>();
            _emojiService = new Mock<IEmojiService>();
        }

        [Test]
        public void ValidCommand_ReturnsGoodResults()
        {
            _store
                .Setup(x => x.ReadAll())
                .Returns(new List<string>
                {
                    "just plain old text",
                    "more plain old text",
                    "one with a url http://www.cool.com",
                    "more"
                });

            _analyzer
                .Setup(x => x.MessageRatePerSecond(It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(60);

            _analyzer
                .Setup(x => x.GetHashtagsFromMessage(It.IsAny<string>()))
                .Returns(new List<string>
                {
                    "#great",
                    "#great",
                    "#great",
                    "#wonderful",
                    "#great"
                });

            _analyzer
                .Setup(x => x.DoesContainUrl(It.IsAny<string>()))
                .Returns(true);

            _analyzer
                .Setup(x => x.DoesContainPhotoUrl(It.IsAny<string>()))
                .Returns(true);

            _analyzer
                .Setup(x => x.GetDomainsFromMessage(It.IsAny<string>()))
                .Returns(new List<string>
                {
                    "www.cool.com",
                    "nice.com",
                    "www.cool.com",
                    "www.cool.com"
                });

            _emojiService
                .Setup(x => x.GetEmojisFromMessage(It.IsAny<string>()))
                .Returns(new List<string>
                {
                    "😀",
                    "😀",
                    "🙈",
                    "😀",
                    "😀",
                });

            var handler = new AnalyzeTweets.CommandHandler(_store.Object, _analyzer.Object, _emojiService.Object);
            var response = handler.HandleWrapper(new AnalyzeTweets.Command());

            Assert.False(response.IsError);
            Assert.AreEqual(4, response.Total);
            Assert.AreEqual(60, response.RatePerSecond);
            Assert.AreEqual(60 * 60, response.RatePerMinute);
            Assert.AreEqual(60 * 60 * 60, response.RatePerHour);
            Assert.AreEqual("😀", response.TopEmoji);
            Assert.AreEqual(100, response.PercentageWithUrl);
            Assert.AreEqual(100, response.PercentageWithPhotoUrl);
            Assert.AreEqual("#great", response.TopHashtag);
            Assert.AreEqual("www.cool.com", response.TopDomain);
            Assert.AreEqual(100, response.PercentageWithEmojis);
        }
    }
}
