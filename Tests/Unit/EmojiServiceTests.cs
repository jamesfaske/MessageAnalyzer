using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using NUnit.Framework;

namespace Tests.Unit
{
    [TestFixture]
    [Parallelizable]
    public class EmojiServiceTests
    {
        private EmojiService _emojiService;

        [OneTimeSetUp]
        public void OneTime()
        {
            _emojiService = new EmojiService();
        }

        [Test]
        public void GetEmojisFromMessage_NoEmojis_ReturnsEmpty()
        {
            var message = "no emojis here";
            var result = _emojiService.GetEmojisFromMessage(message);
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void GetEmojisFromMessage_OneEmoji_ReturnsOne()
        {
            var message = "one 😀­ emoji here";
            var result = _emojiService.GetEmojisFromMessage(message);
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public void GetEmojisFromMessage_MultipleEmojis_ReturnsMultiple()
        {
            var message = "😍 multiple 😀­ emojis 🤑 here 🙈";
            var result = _emojiService.GetEmojisFromMessage(message);
            Assert.AreEqual(4, result.Count);
        }
    }
}
